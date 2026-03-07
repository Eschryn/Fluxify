namespace Fluxify.Dto.Channels.Text.Messages;

public record MessageReactionResponse(
    int Count,
    MessageReactionResponseEmoji Emoji,
    bool Me
);