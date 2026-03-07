namespace Fluxify.Dto.Users.Settings.EmailChange;

public record EmailChangeVerifyNewRequest(string Code, string OriginalProof, string Ticket);