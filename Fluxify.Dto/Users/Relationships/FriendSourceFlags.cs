namespace Fluxify.Dto.Users.Relationships;

[Flags]
public enum FriendSourceFlags : uint
{
    MutualFriends = 1,
    MutualGuilds = 2,
    NoRelation = 4,
}