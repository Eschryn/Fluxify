using Fluxify.Gateway.Model.Data;

namespace Fluxify.Gateway.Model.Dto;

public record GatewaySession(
    string SessionId,
    UserStatus Status,
    bool Mobile,
    bool Afk
);