using Fluxify.Dto.Users;

namespace Fluxify.Gateway.Model.Data.User;

public record Presence(
    UserPartialResponse UserPartial,
    UserStatus Status,
    bool Mobile,
    bool Afk,
    CustomStatus? CustomStatus
);