using System.Net.Mime;

namespace Fluxify.Dto.Channels.Text.Messages;

/// <summary>
/// 
/// </summary>
/// <param name="ContentHash"></param>
/// <param name="ContentType"></param>
/// <param name="Waveform">Base64 encoded waveform data</param>
/// <param name="Duration">Duration in seconds</param>
/// <param name="Placeholder">Base64 encoded placeholder image</param>
public record MessageAttachmentResponse(
    string? ContentHash,
    ContentType? ContentType,
    string? Description,
    int? Duration,
    bool? Expired,
    DateTimeOffset? ExpiresAt,
    string FileName,
    MessageAttachmentFlags Flags,
    int? Height,
    bool? Nsfw,
    string? Placeholder,
    string? ProxyUrl,
    int Size,
    string? Title,
    string? Url,
    string? Waveform,
    int? Width
);