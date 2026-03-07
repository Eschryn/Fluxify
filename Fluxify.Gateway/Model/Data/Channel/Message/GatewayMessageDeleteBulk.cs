using Fluxify.Core;

namespace Fluxify.Gateway.Model.Dto;

public record GatewayMessageDeleteBulk(Snowflake[] Ids, Snowflake ChannelId);