namespace Fluxify.Dto.Channels.Text.Messages.Reactions;

public record MessageReactionResponse(
    int Count,
    MessageReactionResponseEmoji Emoji,
    bool? Me
);