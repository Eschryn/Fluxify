namespace Fluxify.Dto.Webhooks.Sentry;

public record SentryWebhook(
    string? Action,
    SentryWebhookActor? Actor,
    SentryWebhookData? Data,
    SentryWebhookInstallation? Installation
);