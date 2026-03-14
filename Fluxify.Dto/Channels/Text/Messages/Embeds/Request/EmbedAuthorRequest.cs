using System.ComponentModel.DataAnnotations;

namespace Fluxify.Dto.Channels.Text.Messages.Embeds;

/// <summary>
/// The author of the embed
/// </summary>
/// <param name="IconUrl">The author's icon URL.</param>
/// <param name="Name">The author's name.</param>
/// <param name="Url">The author's URL.</param>
public sealed record EmbedAuthorRequest(
    [property: StringLength(2048)]
    string? IconUrl,
    [property: StringLength(256)]
    string Name,
    [property: StringLength(2048)]
    string? Url
);