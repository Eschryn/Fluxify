namespace Fluxify.Gateway.Model.Dto;

public record GatewayAuthSessionChange(
    string OldAuthSessionIdHash,
    string NewAuthSessionIdHash,
    string NewToken
);