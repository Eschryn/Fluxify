using Fluxify.Core;
using Fluxify.Dto.Users;

namespace Fluxify.Gateway.Model.Dto;

public record GatewayGroupChange(Snowflake ChannelId, Snowflake GuildId, UserResponse User);