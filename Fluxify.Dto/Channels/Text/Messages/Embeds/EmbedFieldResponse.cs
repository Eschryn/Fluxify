namespace Fluxify.Dto.Channels.Text.Messages.Embeds;

public record EmbedFieldResponse(
    bool Inline,
    string Name,
    string Value
);