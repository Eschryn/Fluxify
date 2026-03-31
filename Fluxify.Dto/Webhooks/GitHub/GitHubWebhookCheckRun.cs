namespace Fluxify.Dto.Webhooks.GitHub;

public record GitHubWebhookCheckRun(
    GitHubWebhookCheckSuite CheckSuite,
    string? Conclusion,
    Uri? DetailsUrl,
    Uri HtmlUrl,
    string Name,
    GitHubWebhookOutput? Output,
    GitHubWebhookCheckSuitePullRequest[] PullRequests
);