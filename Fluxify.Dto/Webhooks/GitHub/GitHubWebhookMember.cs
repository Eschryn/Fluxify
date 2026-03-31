namespace Fluxify.Dto.Webhooks.GitHub;

public record GitHubWebhookMember(
    Uri? AvatarUrl,
    Uri HtmlUrl,
    int Id,
    string Login
);