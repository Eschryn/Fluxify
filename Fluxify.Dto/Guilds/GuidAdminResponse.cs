using Fluxify.Core.Types;
using Fluxify.Dto.Guilds.Settings;

namespace Fluxify.Dto.Guilds;

public record GuidAdminResponse(
    string? Banner,
    GuildFeatureSchema[] Features,
    string? Icon,
    Snowflake Id,
    int MemberCount,
    string Name,
    Snowflake OwnerId);