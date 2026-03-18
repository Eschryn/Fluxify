using Fluxify.Core.Types;
using Fluxify.Dto.Channels.Text.Messages;

namespace Fluxify.Dto.Reports;

public record ReportMessageRequest(
    string? AdditionalInfo,
    MessageReportCategoryEnum Category,
    Snowflake ChannelId,
    Snowflake MessageId
);