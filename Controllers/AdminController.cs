using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using fruit_api.Data;
using fruit_api.Domain;
using fruit_api.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace fruit_api.Controllers;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
[Route("admin/data")]
public sealed class AdminController(FruitDb db, IDistributedCache cache, IConfiguration config) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List(CancellationToken token)
    {
        if (!Authorized()) return Unauthorized();
        return Ok(await db.Set<StorefrontContent>().AsNoTracking()
            .OrderBy(row => row.Kind).ThenBy(row => row.Id).ToListAsync(token));
    }

    [HttpPut("{kind}/{id}")]
    public async Task<IActionResult> Save(string kind, string id, JsonElement payload, CancellationToken token)
    {
        if (!Authorized()) return Unauthorized();
        if (payload.ValueKind != JsonValueKind.Object || id.Length > 100 ||
            kind is not ("singleton" or "news" or "text") ||
            kind == "singleton" && id is not ("catalog" or "schedule" or "config" or "update"))
            return BadRequest("Проверьте тип записи, идентификатор и JSON.");

        DateTimeOffset? publishedAt = null;
        if (kind == "news")
        {
            if (!payload.TryGetProperty("id", out var key) || key.ValueKind != JsonValueKind.String ||
                key.GetString() != id || !payload.TryGetProperty("publishedAt", out var date) ||
                date.ValueKind != JsonValueKind.String || !date.TryGetDateTimeOffset(out var parsed))
                return BadRequest("Укажите совпадающий id и publishedAt в формате ISO 8601.");
            publishedAt = parsed.ToUniversalTime();
        }

        if (kind == "singleton" && id == "catalog" &&
            new[] { "categories", "products", "sets" }.Any(key =>
                !payload.TryGetProperty(key, out var list) || list.ValueKind != JsonValueKind.Array))
            return BadRequest("Каталог должен содержать массивы categories, products и sets.");

        var json = payload.GetRawText();
        await db.Database.ExecuteSqlInterpolatedAsync($"""
            INSERT INTO storefront_content (kind, id, payload, published_at)
            VALUES ({kind}, {id}, CAST({json} AS jsonb), {publishedAt})
            ON CONFLICT (kind, id) DO UPDATE SET payload = EXCLUDED.payload, published_at = EXCLUDED.published_at
            """, token);

        if (kind == "singleton" && id == "catalog")
            await cache.RemoveAsync(Storefront.CacheKey, token);
        return NoContent();
    }

    [HttpDelete("{kind}/{id}")]
    public async Task<IActionResult> Delete(string kind, string id, CancellationToken token)
    {
        if (!Authorized()) return Unauthorized();
        if (kind is not ("news" or "text")) return BadRequest();
        await db.Set<StorefrontContent>().Where(row => row.Kind == kind && row.Id == id)
            .ExecuteDeleteAsync(token);
        return NoContent();
    }

    [HttpPost("broadcast")]
    public async Task<IActionResult> Broadcast(BroadcastInput input, CancellationToken token)
    {
        if (!Authorized()) return Unauthorized();
        if (string.IsNullOrWhiteSpace(input.Title) || input.Title.Length > 80 ||
            string.IsNullOrWhiteSpace(input.Body) || input.Body.Length > 300)
            return BadRequest("Укажите заголовок до 80 и текст до 300 символов.");

        var firebase = FirebaseAdmin.FirebaseApp.DefaultInstance;
        var topic = config["Firebase:BroadcastTopic"];
        if (firebase is null || string.IsNullOrWhiteSpace(topic))
            return StatusCode(503, "Рассылка недоступна. Настройте Firebase и тему подписки приложения.");

        var messageId = await FirebaseAdmin.Messaging.FirebaseMessaging.GetMessaging(firebase).SendAsync(
            new FirebaseAdmin.Messaging.Message
            {
                Topic = topic,
                Notification = new FirebaseAdmin.Messaging.Notification { Title = input.Title, Body = input.Body }
            }, token);
        return Ok(new { messageId });
    }

    public sealed record BroadcastInput(string Title, string Body);

    private bool Authorized()
    {
        var password = config["Admin:Password"];
        if (string.IsNullOrEmpty(password) ||
            !AuthenticationHeaderValue.TryParse(Request.Headers.Authorization, out var auth) ||
            !auth.Scheme.Equals("Basic", StringComparison.OrdinalIgnoreCase))
            return false;

        var expected = Convert.ToBase64String(Encoding.UTF8.GetBytes($"admin:{password}"));
        return CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(auth.Parameter ?? ""), Encoding.UTF8.GetBytes(expected));
    }
}


