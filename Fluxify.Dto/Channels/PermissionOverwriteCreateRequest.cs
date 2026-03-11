using Fluxify.Core.Types;

namespace Fluxify.Dto.Channels;

record PermissionOverwriteCreateRequest(
    Permissions? Allow,
    Permissions? Deny,
    PermissionOverwriteType Type
);