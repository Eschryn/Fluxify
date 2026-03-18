using Fluxify.Dto.Channels.Text.Messages;

namespace Fluxify.Dto.Reports;

public record DsaReportMessageRequest(
    string? AdditionalInfo,
    MessageReportCategoryEnum? Category,
    string MessageLink,
    string? ReportedUserTag,
    Country ReporterCountryOfResidence,
    string? ReporterFluxerTag,
    string ReporterFullLegalName,
    string Ticket
) : DsaReportRequest(
    AdditionalInfo,
    ReporterCountryOfResidence,
    ReporterFluxerTag,
    ReporterFullLegalName,
    Ticket
);