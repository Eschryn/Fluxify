using Fluxify.Core.Types;

namespace Fluxify.Dto.Guilds;

public record GuildAssetItemSchema(
    bool Animated,
    Snowflake CreatorId,
    Snowflake Id,
    string MediaUrl,
    string Name);