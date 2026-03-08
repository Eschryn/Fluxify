using Fluxify.Dto.Channels;
using Fluxify.Dto.Common;
using Fluxify.Dto.Users;

namespace Fluxify.Dto.Guilds.Invite;

public record GuildInviteMetadataResponse(
    ChannelPartialResponse Channel,
    string Code,
    DateTimeOffset CreatedAt,
    DateTimeOffset? ExpiresAt,
    GuildInviteMetadataResponseGuild Guild,
    User Inviter,
    int MaxAge,
    int MaxUses,
    int MemberCount,
    int PresenceCount,
    bool Temporary,
    InviteType Type,
    int Uses
);