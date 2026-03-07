using Fluxify.Core;

namespace Fluxify.Gateway.Model.Dto;

public record GatewayChannelPinsUpdate(Snowflake ChannelId, DateTimeOffset? LastPinTimestamp);