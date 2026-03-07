namespace Fluxify.Dto.Guilds.Invite;

public enum JoinSourceType
{
    CreatedGuild = 0,
    InstantInvite = 1,
    VanityUrl = 2,
    BotInvite = 3,

    /// <summary>
    /// User was force added to the guild by a platform administrator
    /// </summary>
    PlatformAdministrator = 4
}