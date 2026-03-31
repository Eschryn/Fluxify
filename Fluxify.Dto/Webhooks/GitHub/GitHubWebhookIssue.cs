namespace Fluxify.Dto.Webhooks.GitHub;

public record GitHubWebhookIssue(
    string? Body,
    Uri HtmlUrl,
    long Id,
    int Number,
    string Title,
    GitHubWebhookMember User
);