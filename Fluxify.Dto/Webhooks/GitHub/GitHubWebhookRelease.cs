namespace Fluxify.Dto.Webhooks.GitHub;

public record GitHubWebhookRelease(
    string? Body,
    Uri? HtmlUrl,
    int Id,
    string? TagName
);