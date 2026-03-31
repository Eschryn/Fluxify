namespace Fluxify.Dto.Webhooks.Slack;

public record SlackWebhookRequest(
    SlackWebhookRequestAttachmentsItem[]? Attachments,
    string? IconUrl,
    string? Text,
    string? Username
);