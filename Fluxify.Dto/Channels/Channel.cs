using System.Text.Json.Serialization;
using Fluxify.Core;
using Fluxify.Dto.Users;

namespace Fluxify.Dto.Channels;

public record Channel(
    int? Bitrate,
    Snowflake? GuildId,
    [property: JsonPropertyName("icon")] string? IconHash,
    Snowflake Id,
    Snowflake? LastMessageId,
    DateTimeOffset? LastPinTimestamp,
    string? Name,
    Dictionary<string, string> Nicks,
    bool? Nsfw,
    Snowflake? OwnerId,
    Snowflake? ParentId,
    ChannelPermissionOverwrite[] Overwrites,
    int? Position,
    int? RateLimitPerUser,
    User[]? Recipients,
    Snowflake? RtcRegion,
    string? Topic,
    int Type,
    string? Url,
    int? UserLimit
);