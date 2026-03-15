using Fluxify.Core.Types;

namespace Fluxify.Dto.Guilds.Roles;

public record GuildRoleCreateRequest(
    int? Color,
    string Name,
    Permissions? Permissions
);