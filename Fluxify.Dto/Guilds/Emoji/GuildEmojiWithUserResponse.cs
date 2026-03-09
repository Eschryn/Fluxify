using Fluxify.Core;
using Fluxify.Dto.Users;

namespace Fluxify.Dto.Guilds.Emoji;

public record GuildEmojiWithUserResponse(bool Animated, Snowflake Id, string Name, UserResponse User)
    : GuildEmojiResponse(Animated, Id, Name);