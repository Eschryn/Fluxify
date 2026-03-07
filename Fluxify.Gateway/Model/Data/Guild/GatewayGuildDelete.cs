using Fluxify.Core;

namespace Fluxify.Gateway.Model.Dto;

public record GatewayGuildDelete(
    Snowflake Id,
    bool? Unavailable
);