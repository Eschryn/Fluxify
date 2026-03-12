namespace Fluxify.Application.Model.Messages.Embeds;

public class EmbedAuthor
{
    public required string Name { get; set; }
    public string? IconUrl { get; set; }
    public string? ProxyIconUrl { get; set; }
    public string? Url { get; set; }   
}