namespace Fluxify.Dto.Users.Settings.EmailChange;

public record EmailChangeRequestNewResponse(
    DateTimeOffset NewCodeExpiresAt,
    string NewEmail,
    DateTimeOffset ResendAvailableAt,
    string Ticket
);