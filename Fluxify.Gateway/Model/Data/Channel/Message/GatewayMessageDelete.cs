using Fluxify.Core;

namespace Fluxify.Gateway.Model.Dto;

public record GatewayMessageDelete(
    Snowflake Id,
    Snowflake ChannelId,
    string? Content,
    Snowflake? AuthorId
);