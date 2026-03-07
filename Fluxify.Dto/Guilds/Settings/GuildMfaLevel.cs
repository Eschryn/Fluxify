namespace Fluxify.Dto.Guilds.Settings;

public enum GuildMfaLevel
{
    /// <summary>
    /// Guild has no MFA/2FA requirement
    /// </summary>
    None = 0,

    /// <summary>
    /// Guild requires 2FA for moderation actions
    /// </summary>
    Moderation = 1,
}