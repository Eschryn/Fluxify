using System.ComponentModel.DataAnnotations;

namespace Fluxify.Dto.Channels.Text.Messages.Embeds;

public sealed record EmbedFooterRequest(
    [property: StringLength(2048)]
    string? IconUrl,
    [property: StringLength(2048)]
    string Text
);