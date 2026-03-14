using System.ComponentModel.DataAnnotations;
using Fluxify.Core.Types;

namespace Fluxify.Dto.Channels.Text.Messages.Attachments;

/// <param name="Id">The attachment's ID must correspond to a file upload when creating a message.</param>
/// <param name="Title">The attachment's title.</param>
/// <param name="Description">The attachment's description.</param>
/// <param name="Filename">The attachment's filename.</param>
/// <param name="Flags">The attachment's flags.</param>
public sealed record MessageAttachmentRequest(
    Snowflake Id,
    [property: StringLength(1024)]
    string? Title,
    [property: StringLength(4096)]
    string? Description,
    [property: StringLength(255)]
    string Filename,
    MessageAttachmentFlags Flags
);