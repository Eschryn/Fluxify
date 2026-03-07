using Fluxify.Core;

namespace Fluxify.Gateway.Model.Dto;

public record GatewayVoiceServer(
    string Token,
    string Endpoint,
    string ConnectionId,
    Snowflake GuildId,
    Snowflake? ChannelId
);