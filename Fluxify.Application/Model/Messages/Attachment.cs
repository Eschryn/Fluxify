using Fluxify.Application.Entities.Messages;
using Fluxify.Core.Types;

namespace Fluxify.Application.Model.Messages;

public class Attachment
{
    public Snowflake Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public required string Filename { get; set; }
    public AttachmentFlags Flags { get; set; } = AttachmentFlags.None;
    public string? ContentHash { get; internal set; }
    public string? ContentType { get; internal set; }
    public int? Duration { get; internal set; }
    public bool? Expired { get; internal set; }
    public DateTimeOffset? ExpiresAt { get; internal set; }
    public int? Height { get; internal set; }   
    public bool? Nsfw { get; internal set; }
    public string? Placeholder { get; internal set; }   
    public string? ProxyUrl { get; internal set; }  
    public int Size { get; internal set; }
    public string? Url { get; internal set; }
    public string? Waveform { get; internal set; } 
    public int? Width { get; internal set; }
}