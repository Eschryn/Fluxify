namespace Fluxify.Dto.Webhooks.GitHub;

public record GitHubWebhookCheckSuite(
    GitHubWebhookApp App,
    string? Conclusion,
    string? HeadBranch,
    string HeadSha,
    GitHubWebhookCheckSuitePullRequest[] PullRequests
);