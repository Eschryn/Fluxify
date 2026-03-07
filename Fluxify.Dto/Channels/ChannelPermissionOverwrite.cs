using Fluxify.Core;

namespace Fluxify.Dto.Channels;

public record ChannelPermissionOverwrite(
    ulong? Allow,
    ulong? Deny,
    Snowflake Id,
    PermissionOverwriteType Type
);