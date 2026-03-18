using Fluxify.Core.Types;

namespace Fluxify.Dto.Reports;

public record ReportResponse(
    Snowflake ReportId,
    DateTimeOffset ReportedAt,
    string Status
);