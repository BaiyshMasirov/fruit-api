using System.Text.Json;

namespace fruit_api.Domain;

public sealed class StorefrontContent
{
    public required string Kind { get; init; }
    public required string Id { get; init; }
    public required JsonDocument Payload { get; init; }
    public DateTimeOffset? PublishedAt { get; init; }
}

