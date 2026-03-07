namespace Fluxify.Dto.Gateway;

public record GatewayBotResponse(
    SessionStartLimits SessionStartLimit,
    long Shards,
    string Url
);