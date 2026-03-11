using Fluxify.Core.Types;

namespace Fluxify.Dto.Users.Settings.EmailChange;

public record EmailChangeRequestNewRequest(string NewEmail, string OriginalProof, Snowflake Ticket);