using Fluxify.Core.Types;
using Fluxify.Dto.Users;

namespace Fluxify.Dto.Reports;

public record DsaReportUserRequest(
    string? AdditionalInfo,
    UserReportCategoryEnum? Category,
    Country ReporterCountryOfResidence,
    string? ReporterFluxerTag,
    string ReporterFullLegalName,
    string Ticket,
    Snowflake UserId,
    string? UserTag
) : DsaReportRequest(
    AdditionalInfo,
    ReporterCountryOfResidence,
    ReporterFluxerTag,
    ReporterFullLegalName,
    Ticket
);