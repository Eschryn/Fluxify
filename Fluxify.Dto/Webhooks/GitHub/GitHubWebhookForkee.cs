namespace Fluxify.Dto.Webhooks.GitHub;

public record GitHubWebhookForkee(
    string FullName,
    Uri HtmlUrl,
    int Id,
    string Name
);