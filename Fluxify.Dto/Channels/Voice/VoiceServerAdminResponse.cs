using Fluxify.Core;
using Fluxify.Dto.Guilds.Settings;

namespace Fluxify.Dto.Channels.Voice;

public record VoiceServerAdminResponse(
    Snowflake[]? AllowedGuildIds,
    Snowflake[]? AllowedUserIds,
    DateTimeOffset? CreatedAt,
    string Endpoint,
    bool IsActive,
    Snowflake RegionId,
    GuildFeatureSchema[] RequiredGuildFeatures,
    Snowflake ServerId,
    DateTimeOffset? UpdatedAt,
    bool VipOnly
);