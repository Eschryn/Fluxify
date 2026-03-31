namespace Fluxify.Dto.Webhooks.GitHub;

public record GitHubWebhookAnswer(
    string Body,
    string? CommitId,
    Uri HtmlUrl,
    int Id,
    GitHubWebhookMember User
);