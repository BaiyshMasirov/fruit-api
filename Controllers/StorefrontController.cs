using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using fruit_api.Application;
using fruit_api.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace fruit_api.Controllers;

[ApiController]
[Route("api/v1")]
public sealed class StorefrontController(IStorefront storefront) : ControllerBase
{
    [HttpGet("catalog")]
    public async Task<IActionResult> Catalog(CancellationToken token)
    {
        var catalog = await storefront.ReadCatalog(token);
        var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(catalog)));
        var tag = new EntityTagHeaderValue($"\"{hash}\"");
        Response.GetTypedHeaders().ETag = tag;
        Response.Headers.CacheControl = "no-cache";
        if (Request.GetTypedHeaders().IfNoneMatch.Any(candidate =>
            candidate.Equals(EntityTagHeaderValue.Any) || candidate.Compare(tag, false)))
            return StatusCode(304);

        return Responses.Ok(JsonSerializer.Deserialize<JsonElement>(catalog));
    }

    [HttpGet("config")]
    public Task<IActionResult> Config(CancellationToken token) =>
        Read("singleton", "config", token);

    [HttpGet("app/update")]
    public Task<IActionResult> Update(CancellationToken token) =>
        Read("singleton", "update", token);

    [HttpGet("schedule")]
    public Task<IActionResult> Schedule(CancellationToken token) =>
        Read("singleton", "schedule", token);

    [HttpGet("texts/{key}")]
    public Task<IActionResult> Text(string key, CancellationToken token) =>
        Read("text", key, token);

    [HttpGet("news/{id}")]
    public Task<IActionResult> NewsItem(string id, CancellationToken token) =>
        Read("news", id, token);

    [HttpGet("news")]
    public async Task<IActionResult> News(int? page, int? size, CancellationToken token) =>
        Responses.Ok(await storefront.ReadNews(Math.Max(1, page ?? 1), Math.Clamp(size ?? 20, 1, 50), token));

    private async Task<IActionResult> Read(string kind, string id, CancellationToken token)
    {
        var json = await storefront.Read(kind, id, token);
        return json is null
            ? Responses.Error(404, "not_found", "Не найдено")
            : Responses.Ok(JsonSerializer.Deserialize<JsonElement>(json));
    }
}
