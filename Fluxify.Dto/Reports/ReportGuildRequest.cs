using Fluxify.Core.Types;
using Fluxify.Dto.Guilds;

namespace Fluxify.Dto.Reports;

public record ReportGuildRequest(
    string? AdditionalInfo,
    GuildReportCategoryEnum Category,
    Snowflake GuildId
);