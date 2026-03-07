namespace Fluxify.Dto.Channels.Text.Messages.Embeds;

public record EmbedMediaResponse(
    string? ContentHash,
    string? ContentType,
    string? Description,
    int? Duration,
    EmbedMediaFlags Flags,
    int? Height,
    string? Placeholder,
    string? ProxyUrl,
    string Url,
    int? Width
);