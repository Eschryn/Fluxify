using Fluxify.Core.Types;

namespace Fluxify.Gateway.Model.Data.Voice;

public record GatewayVoiceServer(
    string Token,
    string Endpoint,
    string ConnectionId,
    Snowflake GuildId,
    Snowflake? ChannelId
);