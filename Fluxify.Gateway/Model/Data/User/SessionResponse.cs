namespace Fluxify.Gateway.Model.Data.User;

public record SessionResponse(
    string SessionId,
    UserStatus Status,
    bool Mobile,
    bool Afk
);