using Fluxify.Dto.Common;
using Fluxify.Dto.Users;

namespace Fluxify.Dto.Channels.GroupDm;

public record GroupDmInviteResponse(
    ChannelPartialResponse Channel,
    string Code,
    DateTimeOffset? ExpiresAt,
    UserResponse? Inviter,
    int MemberCount,
    bool Temporary,
    InviteType Type
);