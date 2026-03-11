using Fluxify.Core.Types;

namespace Fluxify.Dto.Guilds.Emoji;

public record GuildEmojiResponse(bool Animated, Snowflake Id, string Name);