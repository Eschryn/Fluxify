namespace Fluxify.Dto.Users;

[Flags]
public enum IncomingCallFlags : uint
{
    FriendsOfFriends = 1,
    GuildMembers = 2,
    Everyone = 4,
    FriendsOnly = 8,
    Nobody = 16,
    SilentEveryone = 32,
}