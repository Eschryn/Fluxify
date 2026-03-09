using Fluxify.Dto.Channels;
using Fluxify.Dto.Common;
using Fluxify.Dto.Users;

namespace Fluxify.Dto.Guilds.Invite;

public record GuildInviteResponse(
    ChannelPartialResponse Channel,
    string Code,
    DateTimeOffset CreatedAt,
    GuildInviteMetadataResponseGuild Guild,
    UserResponse? Inviter,
    int MemberCount,
    int PresenceCount,
    bool Temporary,
    InviteType Type);