using Fluxify.Core;

namespace Fluxify.Dto.Users.GuildSettings;

public record UserGuildSettings(
    Dictionary<string, UserGuildSettingsResponseChannelOverridesAdditionalProperties> ChannelOverrides,
    Snowflake? GuildId,
    bool HideMutedChannels,
    UserNotificationSettings MessageNotifications,
    bool MobilePush,
    UserGuildSettingsResponseMuteConfig? MuteConfig,
    bool Muted,
    bool SuppressEveryone,
    bool SuppressRoles,
    int Version
);