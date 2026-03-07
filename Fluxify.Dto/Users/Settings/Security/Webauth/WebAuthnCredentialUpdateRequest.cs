namespace Fluxify.Dto.Users.Settings.Security.Webauth;

public record WebAuthnCredentialUpdateRequest(
    string? MfaCode,
    MfaMethod? MfaMethod,
    string? Name,
    string? Password,
    string? WebauthnChallenge,
    string? WebauthnResponse
);