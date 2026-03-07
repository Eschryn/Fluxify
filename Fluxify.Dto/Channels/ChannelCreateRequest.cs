using Fluxify.Core;

namespace Fluxify.Dto.Channels;

public abstract record ChannelCreateRequest(
    string Name,
    ChannelType Type,
    Snowflake? ParentId,
    ChannelPermissionOverwrite[]? PermissionOverwrites
);