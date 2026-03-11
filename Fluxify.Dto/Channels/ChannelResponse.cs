using System.Text.Json.Serialization;
using Fluxify.Core.Types;
using Fluxify.Dto.Users;

namespace Fluxify.Dto.Channels;

public record ChannelResponse(
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
    UserResponse[]? Recipients,
    Snowflake? RtcRegion,
    string? Topic,
    ChannelType Type,
    string? Url,
    int? UserLimit
);