using Fluxify.Core;

namespace Fluxify.Dto.Channels.LinkChannel;

public record ChannelUpdateLinkRequest(
    string? Name,
    Snowflake? ParentId,
    ChannelPermissionOverwrite[] PermissionOverwrites,
    string? Url
) : ChannelUpdateRequest;