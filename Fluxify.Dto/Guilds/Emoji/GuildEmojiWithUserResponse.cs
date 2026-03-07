using Fluxify.Core;
using Fluxify.Dto.Users;

namespace Fluxify.Dto.Guilds.Emoji;

public record GuildEmojiWithUserResponse(bool Animated, Snowflake Id, string Name, User User)
    : GuildEmojiResponse(Animated, Id, Name);