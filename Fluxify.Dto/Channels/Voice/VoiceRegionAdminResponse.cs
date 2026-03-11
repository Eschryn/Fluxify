using Fluxify.Core.Types;
using Fluxify.Dto.Guilds.Settings;

namespace Fluxify.Dto.Channels.Voice;

public record VoiceRegionAdminResponse(
    Snowflake[] AllowedGuildIds,
    Snowflake[] AllowedUserIds,
    DateTimeOffset? CreatedAt,
    string Emoji,
    Snowflake Id,
    bool IsDefault,
    double Latitude,
    double Longitude,
    string Name,
    GuildFeatureSchema[] RequiredGuildFeatures,
    DateTimeOffset? UpdatedAt,
    bool VipOnly
);