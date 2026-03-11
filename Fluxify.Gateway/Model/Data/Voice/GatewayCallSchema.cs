using Fluxify.Core.Types;

namespace Fluxify.Gateway.Model.Data.Voice;

public record GatewayCallSchema(
    Snowflake ChannelId,
    Snowflake MessageId,
    string? Region,
    Snowflake[] Ringing,
    VoiceStateResponse[] VoiceStates
);