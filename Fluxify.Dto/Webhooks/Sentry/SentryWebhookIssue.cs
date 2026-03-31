namespace Fluxify.Dto.Webhooks.Sentry;

public record SentryWebhookIssue(
    string Id,
    string ShortId,
    string Title,
    string Permalink,
    string Level,
    string Status,
    string Platform,
    SentryWebhookProject Project,
    string Type,
    SentryWebhookMetadata Metadata,
    string Count,
    string UserCount,
    string FirstSeen,
    string LastSeen,
    string Culprit
);