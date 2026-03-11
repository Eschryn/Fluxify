using Fluxify.Core.Types;
using Fluxify.Dto.Guilds.Emoji;
using Fluxify.Dto.Guilds.Members;

namespace Fluxify.Gateway.Model.Data.Channel.Reaction;

public record GatewayReaction(
    Snowflake ChannelId,
    Snowflake MessageId,
    Snowflake UserId,
    Snowflake? GuildId,
    string? SessionId,
    GuildEmojiResponse Emoji,
    GuildMember? Member
);