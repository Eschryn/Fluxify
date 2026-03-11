using Fluxify.Dto.Users;

namespace Fluxify.Gateway.Model.Data.User;

public record Presence(
    UserResponse User,
    UserStatus Status,
    bool Mobile,
    bool Afk,
    CustomStatus? CustomStatus
);