using System.Text.Json.Serialization;

namespace Fluxify.Dto.Reports;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "report_type")]
[JsonDerivedType(typeof(DsaReportGuildRequest), (int)ReportType.Guild)]
[JsonDerivedType(typeof(DsaReportMessageRequest), (int)ReportType.Message)]
[JsonDerivedType(typeof(DsaReportUserRequest), (int)ReportType.User)]
public record DsaReportRequest(
    string? AdditionalInfo,
    Country ReporterCountryOfResidence,
    string? ReporterFluxerTag,
    string ReporterFullLegalName,
    string Ticket
);