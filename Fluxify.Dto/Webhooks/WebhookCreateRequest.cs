using Fluxify.Dto.Common;

namespace Fluxify.Dto.Webhooks;

public record WebhookCreateRequest(
    Base64Image? Avatar,
    string Name
);