namespace Fluxify.Dto.Users.Settings.Security;

public record SudoVerificationSchema(
    string? MfaCode,
    MfaMethod? MfaMethod,
    string? Password,
    string? WebauthnChallenge,
    string? WebauthnResponse
);