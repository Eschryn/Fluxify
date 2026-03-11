using Fluxify.Core.Types;

namespace Fluxify.Dto.Guilds.Roles;

public record GuildRoleUpdateRequest(
    int? Color,
    bool? Hoist,
    long? HoistPosition,
    bool? Mentionable,
    string? Name,
    Permissions? Permissions
);