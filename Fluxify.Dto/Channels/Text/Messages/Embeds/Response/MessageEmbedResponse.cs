namespace Fluxify.Dto.Channels.Text.Messages.Embeds;

public record MessageEmbedResponse(
    EmbedMediaResponse? Audio,
    EmbedAuthorResponse? Author,
    MessageEmbedChildResponse[]? Children,
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
) : MessageEmbedChildResponse(
    Audio,
    Author, 
    Color,
    Description,
    Fields,
    Footer,
    Image,
    Nsfw,
    Provider,
    Thumbnail,
    Timestamp,
    Title,
    Type,
    Url,
    Video
);