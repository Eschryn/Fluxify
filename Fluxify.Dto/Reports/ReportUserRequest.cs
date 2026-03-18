using Fluxify.Core.Types;
using Fluxify.Dto.Users;

namespace Fluxify.Dto.Reports;

public record ReportUserRequest(
    string? AdditionalInfo,
    UserReportCategoryEnum Category,
    Snowflake? GuildId,
    Snowflake UserId
);