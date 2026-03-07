using Fluxify.Core;

namespace Fluxify.Dto.Channels.Category;

public record ChannelUpdateCategoryRequest(
    int? Bitrate,
    string? Icon,
    string? Name,
    Dictionary<string, string?> Nicks,
    bool? Nsfw,
    Snowflake? OwnerId,
    Snowflake? ParentId,
    ChannelPermissionOverwrite[] PermissionOverwrites,
    int? RateLimitPerUser,
    Snowflake? RtcRegion,
    string? Topic,
    ChannelUpdateCategoryRequest Type,
    int? UserLimit
);