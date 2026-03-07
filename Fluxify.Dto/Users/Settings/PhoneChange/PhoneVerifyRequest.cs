namespace Fluxify.Dto.Users.Settings.PhoneChange;

public record PhoneVerifyRequest(
    string Code,
    string Phone
);