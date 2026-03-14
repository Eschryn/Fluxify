using System.ComponentModel.DataAnnotations;

namespace Fluxify.Dto.Channels.Text.Messages.Embeds;

public sealed record RichEmbedRequest(
    [property: StringLength(2048)]
    string? Url,
    [property: StringLength(256)]
    string? Title,
    [property: Range(0x000000, 0xFFFFFF)]
    int? Color,
    DateTimeOffset? Timestamp,
    [property: StringLength(4096)]
    string? Description,
    EmbedAuthorRequest? Author,
    EmbedMediaRequest? Image,
    EmbedMediaRequest? Thumbnail,
    EmbedFooterRequest? Footer,
    [property: MaxLength(25)]
    EmbedFieldRequest[]? Fields
);