using System.ComponentModel.DataAnnotations;

namespace Fluxify.Dto.Channels.Text.Messages.Embeds;

public sealed record EmbedFieldRequest(
    bool Inline,
    [property: StringLength(256)]
    string Name,
    [property: StringLength(2048)]
    string Value
);