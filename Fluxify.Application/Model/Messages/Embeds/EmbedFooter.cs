namespace Fluxify.Application.Model.Messages.Embeds;

public sealed class EmbedFooter
{
    public string? IconUrl { get; set; }
    public string? ProxyIconUrl { get; internal set; }
    public required string Text { get; init; }
}