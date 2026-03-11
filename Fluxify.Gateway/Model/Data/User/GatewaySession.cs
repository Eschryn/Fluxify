namespace Fluxify.Gateway.Model.Data.User;

public record GatewaySession(
    string SessionId,
    UserStatus Status,
    bool Mobile,
    bool Afk
);