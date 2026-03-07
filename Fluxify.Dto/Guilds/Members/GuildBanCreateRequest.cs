namespace Fluxify.Dto.Guilds.Members;

public record GuildBanCreateRequest(
    long? BanDurationSeconds,
    int? DeleteMessageDays,
    string? Reason);