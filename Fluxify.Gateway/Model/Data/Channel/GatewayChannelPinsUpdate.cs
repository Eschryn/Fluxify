using Fluxify.Core.Types;

namespace Fluxify.Gateway.Model.Data.Channel;

public record GatewayChannelPinsUpdate(Snowflake ChannelId, DateTimeOffset? LastPinTimestamp);