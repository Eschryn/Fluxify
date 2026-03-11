using Fluxify.Core.Types;

namespace Fluxify.Dto.Channels;

public record ChannelPermissionOverwrite(
    Permissions? Allow,
    Permissions? Deny,
    Snowflake Id,
    PermissionOverwriteType Type
);