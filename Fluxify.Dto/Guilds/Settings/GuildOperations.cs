namespace Fluxify.Dto.Guilds.Settings;

[Flags]
public enum GuildOperations : uint
{
    PushNotification = 1,
    EveryoneMentions = 2,
    TypingEvents = 4,
    InstantInvites = 8,
    SendMessage = 16,
    Reactions = 32,
    MemberListUpdates = 64
}