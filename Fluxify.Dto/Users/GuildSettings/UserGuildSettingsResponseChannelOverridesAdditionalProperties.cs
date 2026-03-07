namespace Fluxify.Dto.Users.GuildSettings;

public record UserGuildSettingsResponseChannelOverridesAdditionalProperties(
    bool Collapsed,
    UserNotificationSettings MessageNotifications,
    UserGuildSettingsResponseMuteConfig? MuteConfig,
    bool Muted
);