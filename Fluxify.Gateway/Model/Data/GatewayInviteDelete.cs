using Fluxify.Core.Types;

namespace Fluxify.Gateway.Model.Data;

public record GatewayInviteDelete(string Code, Snowflake? ChannelId, Snowflake? GuildId);