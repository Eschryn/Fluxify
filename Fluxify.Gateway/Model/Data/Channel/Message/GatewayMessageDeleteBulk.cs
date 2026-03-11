using Fluxify.Core.Types;

namespace Fluxify.Gateway.Model.Data.Channel.Message;

public record GatewayMessageDeleteBulk(Snowflake[] Ids, Snowflake ChannelId);