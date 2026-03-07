using Fluxify.Core;

namespace Fluxify.Gateway.Model.Dto;

public record GatewayMessageAck(
    Snowflake ChannelId,
    Snowflake MessageId,
    int MentionCount,
    bool? Manual
);