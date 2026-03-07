namespace Fluxify.Dto.Users.Settings.Security.Mfa;

public record EnableMfaTotpRequest(
    string Code,
    string? MfaCode,
    MfaMethod? MfaMethod,
    string? Password,
    string TotpSecret,
    string? WebauthnChallenge,
    string? WebauthnResponse
);