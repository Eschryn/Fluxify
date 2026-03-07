namespace Fluxify.Dto.Guilds.Settings;

public enum GuildExplicitContentFilter
{
    /// <summary>
    /// Media content will not be scanned
    /// </summary>
    Disabled = 0,

    /// <summary>
    /// Media content from members without roles will be scanned
    /// </summary>
    MembersWithoutRoles = 1,

    /// <summary>
    /// Media content from all members will be scanned
    /// </summary>
    AllMembers = 2
}