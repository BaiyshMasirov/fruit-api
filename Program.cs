using Scalar.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using fruit_api.Data;
using fruit_api.Application;
using fruit_api.Infrastructure;
using fruit_api.Features;

var builder = WebApplication.CreateBuilder(args);
var postgres = builder.Configuration.GetConnectionString("Postgres")
    ?? throw new InvalidOperationException("ConnectionStrings:Postgres is required.");
builder.Services.AddSingleton(_ => NpgsqlDataSource.Create(postgres));
builder.Services.AddDbContext<FruitDb>((services, options) =>
    options.UseNpgsql(services.GetRequiredService<NpgsqlDataSource>()));
builder.Services.AddStackExchangeRedisCache(options =>
    options.Configuration = builder.Configuration.GetConnectionString("Redis") ?? "localhost:6379");
builder.Services.AddScoped<IStorefront, Storefront>();
builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
    options.InvalidModelStateResponseFactory = _ =>
        Responses.Error(400, "validation", "Не удалось обработать запрос"));
builder.Services.AddOpenApi();

var app = builder.Build();
if (!string.IsNullOrWhiteSpace(app.Configuration["Firebase:BroadcastTopic"]) &&
    !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS")))
{
    FirebaseAdmin.FirebaseApp.Create(new FirebaseAdmin.AppOptions
    {
        Credential = Google.Apis.Auth.OAuth2.GoogleCredential.GetApplicationDefault()
    });
}
app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/admin"))
    {
        var adminPort = app.Configuration.GetValue("Admin:Port", 8082);
        if (context.Connection.LocalPort != adminPort)
        {
            context.Response.StatusCode = 404;
            return;
        }
        context.Response.Headers.CacheControl = "no-store";
    }
    await next();
});
app.UseStaticFiles();
app.UseExceptionHandler(error => error.Run(async context =>
    await Responses.Error(500, "server_error", "Ошибка сервера. Попробуйте позже.").ExecuteResultAsync(new Microsoft.AspNetCore.Mvc.ActionContext { HttpContext = context })));
app.UseStatusCodePages(async status =>
{
    var context = status.HttpContext;
    var code = context.Response.StatusCode;
    await Responses.Error(code, code == 404 ? "not_found" : "validation",
        code == 404 ? "Не найдено" : "Не удалось обработать запрос").ExecuteResultAsync(new Microsoft.AspNetCore.Mvc.ActionContext { HttpContext = context });
});
app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/api/v1"))
        context.Response.Headers.CacheControl = "no-store";
    await next();
});

using (var scope = app.Services.CreateScope())
    await scope.ServiceProvider.GetRequiredService<FruitDb>().Database.MigrateAsync();

app.MapMethods("/", ["GET", "HEAD"], () => Results.Ok());
app.MapOpenApi();
app.MapScalarApiReference();
app.MapGet("/admin", () => Results.Redirect("/admin/index.html"));
app.MapControllers();
await app.RunAsync();





