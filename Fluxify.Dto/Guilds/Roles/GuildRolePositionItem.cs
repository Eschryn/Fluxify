using Fluxify.Core;

namespace Fluxify.Dto.Guilds.Roles;

public record GuildRolePositionItem(
    long Position,
    Snowflake Id
);