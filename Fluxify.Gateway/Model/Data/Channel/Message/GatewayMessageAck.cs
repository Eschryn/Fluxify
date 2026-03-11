using Fluxify.Core.Types;

namespace Fluxify.Gateway.Model.Data.Channel.Message;

public record GatewayMessageAck(
    Snowflake ChannelId,
    Snowflake MessageId,
    int MentionCount,
    bool? Manual
);