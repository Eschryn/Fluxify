namespace Fluxify.Dto.Webhooks.GitHub;

public record GitHubWebhookReview(
    string? Body,
    Uri? HtmlUrl,
    string State,
    GitHubWebhookMember User
);