namespace Fluxify.Application.Model.Messages.Embeds;

public class EmbedMedia
{
    public string? ContentHash { get; set; }
    public string? ContentType { get; set; }
    public string? Description { get; set; }
    public int? Duration { get; set; }
    public EmbedMediaFlags Flags { get; set; }
    public int? Height { get; set; }
    public int? Width { get; set; }
    public string? Placeholder { get; set; }
    public required string Url { get; set; }
    public string? ProxyUrl { get; set; }
}