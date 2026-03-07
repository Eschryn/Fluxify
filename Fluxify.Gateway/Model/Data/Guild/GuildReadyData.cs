using System.Text.Json.Serialization;
using Fluxify.Core;

namespace Fluxify.Gateway.Model.Dto;

public record GuildReadyData(
    Snowflake Id,
    bool? Unavailable,
    string? Name,
    [property: JsonPropertyName("icon")] string? IconHash,
    Snowflake? OwnerId,
    int? MemberCount,
    bool? Lazy,
    bool? Large,
    DateTimeOffset? JoinedAt
);