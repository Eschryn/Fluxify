using Fluxify.Core.Types;

namespace Fluxify.Dto.Channels.LinkChannel;

public record ChannelCreateLinkRequest(
    string Name,
    Snowflake? ParentId,
    ChannelPermissionOverwrite[]? PermissionOverwrites,
    string? Url
) : ChannelCreateRequest(
    Name,
    ParentId,
    PermissionOverwrites);