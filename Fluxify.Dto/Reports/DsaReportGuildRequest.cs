using Fluxify.Core.Types;
using Fluxify.Dto.Guilds;

namespace Fluxify.Dto.Reports;

public record DsaReportGuildRequest(
    Snowflake GuildId,
    GuildReportCategoryEnum Category,
    string? AdditionalInfo,
    string? InviteCode,
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