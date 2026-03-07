using Fluxify.Core;
using Fluxify.Dto.Guilds.Members;

namespace Fluxify.Gateway.Model.Dto;

public record GatewayTypingStart(
    Snowflake ChannelId,
    Snowflake UserId,
    DateTimeOffset Timestamp,
    Snowflake? GuildId,
    GuildMember? Member
);