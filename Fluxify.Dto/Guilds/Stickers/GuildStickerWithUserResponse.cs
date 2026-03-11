using Fluxify.Core.Types;
using Fluxify.Dto.Users;

namespace Fluxify.Dto.Guilds.Stickers;

public record GuildStickerWithUserResponse(
    bool Animated,
    string Description,
    Snowflake Id,
    string Name,
    string[] Tags,
    UserResponse User
) : GuildStickerResponse(Animated, Description, Id, Name, Tags);