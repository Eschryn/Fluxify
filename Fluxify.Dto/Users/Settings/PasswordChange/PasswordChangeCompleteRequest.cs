namespace Fluxify.Dto.Users.Settings.PasswordChange;

public record PasswordChangeCompleteRequest(
    string NewPassword,
    string Ticket,
    string VerificationProof
);