namespace Fluxify.Dto.Guilds;

public record GuildDeleteRequest(
    string? MfaCode,
    MfaMethod? MfaMethod,
    string? Password,
    string? WebauthnChallenge,
    string? WebauthnResponse
);