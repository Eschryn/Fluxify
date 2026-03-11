using Fluxify.Core.Types;

namespace Fluxify.Gateway.Model.Data.Guild;

public record GatewayGuildMemberDelete(GatewayGuildMemberDelete.GuildMemberDeleteResponse User)
{
    public record GuildMemberDeleteResponse(Snowflake Id);
}