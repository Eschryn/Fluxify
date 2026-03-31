namespace Fluxify.Dto.Webhooks.Slack;

public record SlackWebhookRequestAttachmentsItem(
    Uri? AuthorIcon,
    Uri? AuthorLink,
    string AuthorName,
    string? Color,
    string? Fallback,
    SlackWebhookRequestAttachmentsItemField[]? Fields,
    string? Footer,
    Uri? ImageUrl,
    string? Pretext,
    string? Text,
    Uri? ThumbUrl,
    string? Title,
    Uri? TitleLink,
    long Ts
);