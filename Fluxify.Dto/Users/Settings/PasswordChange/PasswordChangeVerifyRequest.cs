namespace Fluxify.Dto.Users.Settings.PasswordChange;

public record PasswordChangeVerifyRequest(
    string Code,
    string Ticket
);