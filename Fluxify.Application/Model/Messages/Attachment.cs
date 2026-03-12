using Fluxify.Application.Entities.Messages;
using Fluxify.Core.Types;

namespace Fluxify.Application.Model.Messages;

public class Attachment
{
    public string? ContentHash { get; set; }
    public string? ContentType { get; set; }
    public string? Description { get; set; }
    public int? Duration { get; set; }
    public bool? Expired { get; set; }
    public DateTimeOffset? ExpiresAt { get; set; }
    public required string Filename { get; set; }
    public AttachmentFlags Flags { get; set; }
    public int? Height { get; set; }   
    public bool? Nsfw { get; set; }
    public string? Placeholder { get; set; }   
    public string? ProxyUrl { get; set; }  
    public int Size { get; set; }
    public string? Title { get; set; }
    public string? Url { get; set; }
    public string? Waveform { get; set; } 
    public int? Width { get; set; }
    public Snowflake Id { get; set; }
}