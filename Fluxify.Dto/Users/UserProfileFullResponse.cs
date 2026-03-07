using Fluxify.Dto.Guilds.Members;
using Fluxify.Dto.Users.Relationships;

namespace Fluxify.Dto.Users;

public record UserProfileFullResponse(
    ConnectionResponse[]? ConnectedAccounts,
    GuildMember? GuildMember,
    UserProfile? GuildMemberProfile,
    User[]? Friends,
    MutualGuildResponse[]? MutualGuilds,
    int? PremiumLifetimeSequence,
    DateTimeOffset? PremiumSince,
    UserPremiumTypes? PremiumType,
    User User,
    UserProfile UserProfile
);