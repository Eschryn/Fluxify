namespace Fluxify.Dto.Users.GuildSettings;

public record UserGuildSettingsUpdateRequest(
    Dictionary<string, UserGuildSettingsResponseChannelOverridesAdditionalProperties>? ChannelOverrides,
    bool? HideMutedChannels,
    UserNotificationSettings? MessageNotifications,
    bool? MobilePush,
    UserGuildSettingsResponseMuteConfig? MuteConfig,
    bool? Muted,
    bool? SuppressEveryone,
    bool? SuppressRoles
);