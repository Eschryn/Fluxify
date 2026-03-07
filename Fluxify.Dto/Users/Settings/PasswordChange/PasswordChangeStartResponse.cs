namespace Fluxify.Dto.Users.Settings.PasswordChange;

public record PasswordChangeStartResponse(
    DateTimeOffset CodeExpiresAt,
    DateTimeOffset? ResendAvailableAt,
    string Ticket
);