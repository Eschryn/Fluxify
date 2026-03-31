using Fluxify.Dto.Common;

namespace Fluxify.Dto.Webhooks;

public record WebhookTokenUpdateRequest(
    Base64Image? Avatar,
    string? Name
);