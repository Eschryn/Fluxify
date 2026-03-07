using Fluxify.Core;

namespace Fluxify.Gateway.Model.Dto;

public record GatewayGuildMemberDelete(GatewayGuildMemberDelete.GuildMemberDeleteResponse User)
{
    public record GuildMemberDeleteResponse(Snowflake Id);
}