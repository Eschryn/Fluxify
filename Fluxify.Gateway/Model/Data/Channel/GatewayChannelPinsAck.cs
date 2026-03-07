using Fluxify.Core;

namespace Fluxify.Gateway.Model.Dto;

public record GatewayChannelPinsAck(Snowflake ChannelId, DateTimeOffset Timestamp);