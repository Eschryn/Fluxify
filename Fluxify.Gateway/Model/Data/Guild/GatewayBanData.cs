using Fluxify.Core;
using Fluxify.Dto.Users;

namespace Fluxify.Gateway.Model.Dto;

public record GatewayBanData(
    Snowflake GuildId,
    UserResponse User
);