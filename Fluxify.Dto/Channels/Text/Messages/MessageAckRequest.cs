namespace Fluxify.Dto.Channels.Text.Messages;

public record MessageAckRequest(
    bool? Manual,
    int? MentionCount
);