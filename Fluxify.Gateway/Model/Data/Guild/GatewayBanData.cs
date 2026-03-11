using Fluxify.Core.Types;
using Fluxify.Dto.Users;

namespace Fluxify.Gateway.Model.Data.Guild;

public record GatewayBanData(
    Snowflake GuildId,
    UserResponse User
);