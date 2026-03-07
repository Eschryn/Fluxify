namespace Fluxify.Dto.Users.GuildSettings;

public record UserGuildSettingsResponseMuteConfig(
    DateTimeOffset EndTime,
    int SelectedTimeWindow
);