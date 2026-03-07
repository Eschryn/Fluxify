namespace Fluxify.Dto.Users.Settings.EmailChange;

public record EmailChangeStartResponse(
    string? OriginalCodeExpiresAt,
    string? OriginalEmail,
    string? OriginalProof,
    bool RequireOriginal,
    string? ResendAvailableAt,
    string Ticket
);