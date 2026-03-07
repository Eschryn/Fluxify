using Fluxify.Core;
using Fluxify.Dto.Guilds.Emoji;
using Fluxify.Dto.Guilds.Members;

namespace Fluxify.Gateway.Model.Dto;

public record GatewayReaction(
    Snowflake ChannelId,
    Snowflake MessageId,
    Snowflake UserId,
    Snowflake? GuildId,
    string? SessionId,
    GuildEmojiResponse Emoji,
    GuildMember? Member
);