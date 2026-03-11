using Fluxify.Core.Types;

namespace Fluxify.Gateway.Model.Data.Channel.Message;

public record GatewayMessageDelete(
    Snowflake Id,
    Snowflake ChannelId,
    string? Content,
    Snowflake? AuthorId
);