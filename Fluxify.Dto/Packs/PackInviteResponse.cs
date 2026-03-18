using Fluxify.Dto.Common;
using Fluxify.Dto.Invites;
using Fluxify.Dto.Users;

namespace Fluxify.Dto.Packs;

public record PackInviteResponse(
    string Code,
    DateTimeOffset? ExpiresAt,
    UserPartialResponse? Inviter,
    PackInviteMetadataResponsePack Pack,
    bool Temporary
) : InviteResponseSchema(
    Code,
    Inviter,
    Temporary
);