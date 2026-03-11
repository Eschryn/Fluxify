using Fluxify.Core.Types;

namespace Fluxify.Dto.Guilds.Roles;

public record GuildRolePositionItem(
    long Position,
    Snowflake Id
);