using Fluxify.Core;

namespace Fluxify.Dto.Guilds.Roles;

/// <summary>
/// 
/// </summary>
/// <param name="Color"></param>
/// <param name="Hoist"> Role is displayed seperately in member list</param>
/// <param name="???"></param>
public record GuildRoleResponse(
    int Color,
    bool Hoist,
    long HoistPosition,
    Snowflake Id,
    bool Mentionable,
    string Name,
    ulong Permissions,
    long Position,
    string? UnicodeEmoji
);