using System.ComponentModel.DataAnnotations;

namespace Fluxify.Dto.Channels.Text.Messages.Embeds;

public sealed record EmbedMediaRequest(
    [property: StringLength(4096)] 
    string? Description,
    [property: StringLength(2048)]
    string Url
);