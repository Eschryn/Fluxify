namespace Fluxify.Dto.Users.Settings.Security.Mfa;

public record DisableTotpRequest(
    string Code,
    string? MfaCode,
    MfaMethod? MfaMethod,
    string? Password,
    string? WebauthnChallenge,
    string? WebauthnResponse
);