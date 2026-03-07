using Fluxify.Core;
using Fluxify.Dto.Guilds.Settings;

namespace Fluxify.Dto.Channels.Voice;

public record VoiceRegionWithServersResponse(
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
    VoiceServerAdminResponse[] Servers,
    DateTimeOffset? UpdatedAt,
    bool VipOnly
);