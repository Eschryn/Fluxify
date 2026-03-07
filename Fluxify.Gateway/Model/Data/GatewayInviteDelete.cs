using Fluxify.Core;

namespace Fluxify.Gateway.Model.Dto;

public record GatewayInviteDelete(string Code, Snowflake? ChannelId, Snowflake? GuildId);