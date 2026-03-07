namespace Fluxify.Dto.Guilds.Roles;

public record GuildRoleCreateRequest(
    int Color,
    string Name,
    ulong Permissions
);