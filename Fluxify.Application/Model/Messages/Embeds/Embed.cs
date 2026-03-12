using System.Drawing;

namespace Fluxify.Application.Model.Messages.Embeds;

public class Embed
{
    public EmbedMedia? Audio { get; set; }
    public EmbedMedia? Thumbnail { get; set; }
    public EmbedMedia? Video { get; set; }
    public EmbedMedia? Image { get; set; }
    public EmbedFooter? Footer { get; set; }
    public EmbedField[]? Fields { get; set; }
    public EmbedAuthor? Author { get; set; }
    public string? Title { get; set; }
    public EmbedType Type { get; set; } = EmbedType.Rich;
    public string? Description { get; set; }
    public string? Url { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public Color? Color { get; set; }
    public bool? Nsfw { get; set; }
    public EmbedAuthor? Provider { get; set; }
}