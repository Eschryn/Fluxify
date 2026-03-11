using Fluxify.Core.Types;

namespace Fluxify.Gateway.Model.Data.Channel;

public record GatewayChannelPinsAck(Snowflake ChannelId, DateTimeOffset Timestamp);