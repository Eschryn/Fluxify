namespace Fluxify.Dto.Users.Settings.Security.Webauth;

public record WebAuthnRegisterRequest(
    string Challenge,
    string? MfaCode,
    MfaMethod? MfaMethod,
    string? Name,
    string? Password,
    string Response,
    string? WebauthnChallenge,
    string? WebauthnResponse
);