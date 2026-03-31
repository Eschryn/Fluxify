namespace Fluxify.Dto.Webhooks.GitHub;

public record GitHubWebhookCommitsItem(
    GitHubWebhookCommitAuthor Author,
    string Id,
    string Message,
    Uri Url
);