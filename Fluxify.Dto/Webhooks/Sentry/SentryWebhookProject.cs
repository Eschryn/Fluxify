namespace Fluxify.Dto.Webhooks.Sentry;

public record SentryWebhookProject(
    string Id,
    string Name,
    string Slug,
    string Platform
);