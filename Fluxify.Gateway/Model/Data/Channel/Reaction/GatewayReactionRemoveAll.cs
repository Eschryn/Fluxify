using Fluxify.Core.Types;

namespace Fluxify.Gateway.Model.Data.Channel.Reaction;

public record GatewayReactionRemoveAll(Snowflake ChannelId, Snowflake MessageId);