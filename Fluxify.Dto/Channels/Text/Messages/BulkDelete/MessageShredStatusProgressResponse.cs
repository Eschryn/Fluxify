namespace Fluxify.Dto.Channels.Text.Messages.BulkDelete;

public record MessageShredStatusProgressResponse(
    string? CompletedAt,
    string? Error,
    string? FailedAt,
    int Processed,
    int Requested,
    int Skipped,
    string? StartedAt,
    MessageShredStatusProgressResponseStatus Status,
    int Total
);