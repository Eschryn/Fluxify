using Fluxify.Core;

namespace Fluxify.Dto.Channels.LinkChannel;

public record ChannelCreateLinkRequest(
    string Name,
    Snowflake? ParentId,
    ChannelPermissionOverwrite[]? PermissionOverwrites,
    string? Url
) : ChannelCreateRequest(
    Name,
    ChannelType.LinkChannel,
    ParentId,
    PermissionOverwrites);