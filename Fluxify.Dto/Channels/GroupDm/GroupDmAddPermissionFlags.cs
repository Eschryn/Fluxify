namespace Fluxify.Dto.Channels.GroupDm;

[Flags]
public enum GroupDmAddPermissionFlags : uint
{
    FriendsOfFriends = 1,
    GuildMembers = 2,
    Everyone = 4,
    FriendsOnly = 8,
    Nobody = 16
}