namespace Fluxify.Gateway.Model.Data.User;

public record GatewayAuthSessionChange(
    string OldAuthSessionIdHash,
    string NewAuthSessionIdHash,
    string NewToken
);