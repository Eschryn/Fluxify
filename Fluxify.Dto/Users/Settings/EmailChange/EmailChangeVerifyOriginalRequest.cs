namespace Fluxify.Dto.Users.Settings.EmailChange;

public record EmailChangeVerifyOriginalRequest(string Code, string Ticket);