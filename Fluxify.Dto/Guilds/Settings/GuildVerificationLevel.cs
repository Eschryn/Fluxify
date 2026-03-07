namespace Fluxify.Dto.Guilds.Settings;

public enum GuildVerificationLevel
{
    Unrestricted = 0,
    VerifiedEmail = 1,
    RegisteredLonger5Minutes = 2,
    MemberLonger10Minutes = 3,
    VerifiedPhoneNumber = 4,
}