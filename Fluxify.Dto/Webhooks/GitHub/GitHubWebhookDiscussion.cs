namespace Fluxify.Dto.Webhooks.GitHub;

public record GitHubWebhookDiscussion(
    Uri? AnswerHtmlUrl,
    string? Body,
    Uri HtmlUrl,
    int Number,
    string Title,
    GitHubWebhookMember User
);