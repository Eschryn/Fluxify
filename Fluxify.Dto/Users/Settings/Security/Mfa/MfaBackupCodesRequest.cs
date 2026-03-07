namespace Fluxify.Dto.Users.Settings.Security.Mfa;

public record MfaBackupCodesRequest(
    string? MfaCode,
    MfaMethod? MfaMethod,
    string? Password,
    bool Regenerate,
    string? WebauthnChallenge,
    string? WebauthnResponse
);