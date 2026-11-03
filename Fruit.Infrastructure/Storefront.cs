using fruit_api.Application;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Dapper;
using Microsoft.Extensions.Caching.Distributed;

using Npgsql;

namespace fruit_api.Infrastructure;

public sealed class Storefront(NpgsqlDataSource source, IDistributedCache cache, ILogger<Storefront> logger) : IStorefront
{
    public async Task<string?> Read(string kind, string id, CancellationToken token)
    {
        await using var connection = await source.OpenConnectionAsync(token);
        return await connection.QuerySingleOrDefaultAsync<string>(new CommandDefinition("""
            SELECT payload::text FROM storefront_content WHERE kind = @kind AND id = @id AND (kind <> 'news' OR published_at <= now())
            """, new { kind, id }, cancellationToken: token));
    }

    public async Task<string> ReadCatalog(CancellationToken token)
    {
        const string key = "fruit:v1:catalog";
        try
        {
            var cached = await cache.GetStringAsync(key, token);
            if (cached is not null)
                return cached;
        }
        catch (StackExchange.Redis.RedisException exception)
        {
            logger.LogWarning(exception, "Could not read catalog from Redis");
        }

        var catalog = await Read("singleton", "catalog", token)
            ?? throw new InvalidOperationException("Catalog is missing.");
        try
        {
            await cache.SetStringAsync(key, catalog,
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30) },
                token);
        }
        catch (StackExchange.Redis.RedisException exception)
        {
            logger.LogWarning(exception, "Could not cache catalog in Redis");
        }

        return catalog;
    }

    public async Task<object> ReadNews(int page, int size, CancellationToken token)
    {
        await using var connection = await source.OpenConnectionAsync(token);
        await using var transaction = await connection.BeginTransactionAsync(
            System.Data.IsolationLevel.RepeatableRead, token);
        using var results = await connection.QueryMultipleAsync(new CommandDefinition("""
            SELECT count(*) FROM storefront_content WHERE kind = 'news' AND published_at <= now();
            SELECT (payload - 'blocks' - 'action')::text FROM storefront_content
            WHERE kind = 'news' AND published_at <= now()
            ORDER BY published_at DESC, id OFFSET @offset LIMIT @size;
            """, new { offset = ((long)page - 1) * size, size }, transaction, cancellationToken: token));
        var count = await results.ReadSingleAsync<long>();
        var items = (await results.ReadAsync<string>()).Select(Parse).ToArray();
        await transaction.CommitAsync(token);
        return new { totalPages = (count + size - 1) / size, items };
    }

    internal static JsonElement Parse(string json)
    {
        using var document = JsonDocument.Parse(json);
        return document.RootElement.Clone();
    }
}


