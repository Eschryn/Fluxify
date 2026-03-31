using Fluxify.Core.Types;
using Fluxify.Dto.Common;

namespace Fluxify.Dto.Webhooks;

public record WebhookUpdateRequest(
    Base64Image? Avatar,
    Snowflake? ChannelId,
    string? Name
) : WebhookTokenUpdateRequest(
    Avatar,
    Name
);