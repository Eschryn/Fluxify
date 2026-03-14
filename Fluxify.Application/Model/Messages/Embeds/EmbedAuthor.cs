namespace Fluxify.Application.Model.Messages.Embeds;

public sealed class EmbedAuthor
{
    public required string Name { get; init; }
    public string? IconUrl { get; set; }
    public string? ProxyIconUrl { get; internal set; }
    public string? Url { get; set; }   
}