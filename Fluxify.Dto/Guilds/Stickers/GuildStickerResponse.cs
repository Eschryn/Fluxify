using Fluxify.Core.Types;

namespace Fluxify.Dto.Guilds.Stickers;

public record GuildStickerResponse(
    bool Animated,
    string Description,
    Snowflake Id,
    string Name,
    string[] Tags
);