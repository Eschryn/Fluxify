namespace Fluxify.Dto.Users.Settings.Security.Mfa;

public record SudoMfaMethodsResponse(
    bool HasMfa,
    bool Sms,
    bool Totp,
    bool Webauthn
);