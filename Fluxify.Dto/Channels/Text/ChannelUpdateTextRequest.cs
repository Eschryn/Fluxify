using Fluxify.Core;

namespace Fluxify.Dto.Channels.Text;

public record ChannelUpdateTextRequest(
    string? Icon,
    string? Name,
    Dictionary<string, string?> Nicks,
    bool? Nsfw,
    Snowflake? OwnerId,
    Snowflake? ParentId,
    ChannelPermissionOverwrite[] PermissionOverwrites,
    int? RateLimitPerUser,
    string? Topic,
    string? Url,
    ChannelType Type = ChannelType.TextChannel
);