using System.Drawing;

namespace Fluxify.Application.Model.Messages.Embeds;

public sealed class Embed
{
    public string? Url { get; set; }
    public string? Title { get; set; }
    public Color? Color { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public string? Description { get; set; }
    public EmbedAuthor? Author { get; set; }
    public EmbedMedia? Image { get; set; }
    public EmbedMedia? Thumbnail { get; set; }
    public EmbedFooter? Footer { get; set; }
    public EmbedField[]? Fields { get; set; }
    public bool? Nsfw { get; internal set; }
    public EmbedMedia? Video { get; internal set; }
    public EmbedMedia? Audio { get; internal set; }
    public EmbedType Type { get; internal set; } = EmbedType.Rich;
    public EmbedAuthor? Provider { get; internal set; }
}