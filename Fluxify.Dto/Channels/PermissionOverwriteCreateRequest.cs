namespace Fluxify.Dto.Channels;

record PermissionOverwriteCreateRequest(
    ulong? Allow,
    ulong? Deny,
    PermissionOverwriteType Type
);