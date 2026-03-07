using Fluxify.Core;

namespace Fluxify.Gateway.Model.Dto;

public record GatewayReactionRemoveAll(Snowflake ChannelId, Snowflake MessageId);