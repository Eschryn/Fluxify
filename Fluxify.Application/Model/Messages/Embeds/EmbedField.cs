namespace Fluxify.Application.Model.Messages.Embeds;

public sealed class EmbedField
{
    public required string Name { get; init; }
    public required string Value { get; init; }
    public bool Inline { get; set; }
}