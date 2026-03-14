using System.ComponentModel.DataAnnotations;

namespace Fluxify.Application.Model.Messages.Embeds;

public sealed class EmbedMedia
{
    public string? ContentHash { get; internal set; }
    public string? ContentType { get; internal set; }
    public string? Description { get; set; }
    public int? Duration { get; internal set; }
    public EmbedMediaFlags Flags { get; internal set; }
    public int? Height { get; internal set; }
    public int? Width { get; internal set; }
    public string? Placeholder { get; internal set; }
    public required string Url { get; init; }
    public string? ProxyUrl { get; internal set; }
}