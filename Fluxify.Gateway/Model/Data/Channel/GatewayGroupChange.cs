using Fluxify.Core.Types;
using Fluxify.Dto.Users;

namespace Fluxify.Gateway.Model.Data.Channel;

public record GatewayGroupChange(Snowflake ChannelId, Snowflake GuildId, UserResponse User);