using Fluxify.Core.Types;
using Fluxify.Dto.Users;

namespace Fluxify.Dto.Guilds.Emoji;

public record GuildEmojiWithUserResponse(bool Animated, Snowflake Id, string Name, UserPartialResponse UserPartial)
    : GuildEmojiResponse(Animated, Id, Name);