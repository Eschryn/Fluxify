namespace Fluxify.Dto.Channels.Text.Messages.Embeds;

public record MessageEmbedChildResponse(
    EmbedMediaResponse? Audio,
    EmbedAuthorResponse? Author,
    int? Color,
    string? Description,
    EmbedFieldResponse[]? Fields,
    EmbedFooterResponse? Footer,
    EmbedMediaResponse? Image,
    bool? Nsfw,
    EmbedAuthorResponse? Provider,
    EmbedMediaResponse? Thumbnail,
    DateTimeOffset? Timestamp,
    string? Title,
    MessageEmbedType Type,
    string? Url,
    EmbedMediaResponse? Video
);