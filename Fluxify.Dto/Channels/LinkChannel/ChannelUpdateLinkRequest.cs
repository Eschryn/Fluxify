using Fluxify.Core;

namespace Fluxify.Dto.Channels.LinkChannel;

public record ChannelUpdateLinkRequest(
    string? Name,
    Snowflake? ParentId,
    Snowflake? OwnerId,
    ChannelPermissionOverwrite[] PermissionOverwrites,
    string? Topic,
    string? Url,
    ChannelType Type = ChannelType.LinkChannel
);