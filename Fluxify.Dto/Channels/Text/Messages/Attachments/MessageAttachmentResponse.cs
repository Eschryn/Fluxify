using Fluxify.Core.Types;

namespace Fluxify.Dto.Channels.Text.Messages.Attachments;

/// <summary>
/// 
/// </summary>
/// <param name="ContentHash"></param>
/// <param name="ContentType"></param>
/// <param name="Waveform">Base64 encoded waveform data</param>
/// <param name="Duration">Duration in seconds</param>
/// <param name="Placeholder">Base64 encoded placeholder image</param>
public record MessageAttachmentResponse(
    Snowflake Id,
    string Filename,
    string? Title,
    string? Description,
    string? ContentType,
    string? ContentHash,
    int Size,
    string? Url,
    string? ProxyUrl,
    int? Width,
    int? Height,
    string? Placeholder,
    MessageAttachmentFlags Flags,
    bool? Nsfw,
    int? Duration,
    string? Waveform,
    bool? Expired,
    DateTimeOffset? ExpiresAt
);