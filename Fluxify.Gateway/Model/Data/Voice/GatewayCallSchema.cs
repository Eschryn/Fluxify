using Fluxify.Core;

namespace Fluxify.Gateway.Model.Dto;

public record GatewayCallSchema(
    Snowflake ChannelId,
    Snowflake MessageId,
    string? Region,
    Snowflake[] Ringing,
    VoiceStateResponse[] VoiceStates
);