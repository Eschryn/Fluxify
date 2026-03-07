using Fluxify.Core;
using Fluxify.Dto.Users;

namespace Fluxify.Dto.Guilds.Stickers;

public record GuildStickerWithUserResponse(
    bool Animated,
    string Description,
    Snowflake Id,
    string Name,
    string[] Tags,
    User User
) : GuildStickerResponse(Animated, Description, Id, Name, Tags);