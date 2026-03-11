using Fluxify.Core.Types;

namespace Fluxify.Gateway.Model.Data.Guild;

public record GatewayGuildDelete(
    Snowflake Id,
    bool? Unavailable
);