using Fluxify.Dto.Common;
using Fluxify.Dto.Users;

namespace Fluxify.Dto.Channels.GroupDm;

public record GroupDmInviteMetadataResponse(
    ChannelPartialResponse Channel,
    string Code,
    DateTimeOffset CreatedAt,
    DateTimeOffset? ExpiresAt,
    User? Inviter,
    int MaxUses,
    int MemberCount,
    bool Temporary,
    InviteType Type,
    int Uses
);