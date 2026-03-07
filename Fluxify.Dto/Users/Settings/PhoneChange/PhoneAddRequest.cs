namespace Fluxify.Dto.Users.Settings.PhoneChange;

public record PhoneAddRequest(
    string MfaCode,
    MfaMethod MfaMethod,
    string? Password,
    string PhoneToken,
    string? WebauthnChallenge,
    string? WebauthnResponse
);