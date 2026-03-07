using Fluxify.Dto.Channels;
using Fluxify.Dto.Users;

namespace Fluxify.Dto.Guilds.Invite;

public record GuildInviteResponse(
    ChannelPartialResponse Channel,
    string Code,
    DateTimeOffset CreatedAt,
    GuildInviteMetadataResponseGuild Guild,
    User? Inviter,
    int MemberCount,
    int PresenceCount,
    bool Temporary,
    InviteType Type);