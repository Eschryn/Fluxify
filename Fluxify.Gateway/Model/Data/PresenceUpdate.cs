using Fluxify.Dto.Users;

namespace Fluxify.Gateway.Model.Data;

public record PresenceUpdate(
    UserStatus Status,
    bool? Mobile = false,
    bool? Afk = false,
    CustomStatus? CustomStatus = null
);