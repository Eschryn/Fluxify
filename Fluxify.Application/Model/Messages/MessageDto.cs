using Fluxify.Application.Entities.Guilds;
using Fluxify.Application.Entities.Messages;
using Fluxify.Application.Model.Messages.Embeds;
using Fluxify.Dto.Uploads;

namespace Fluxify.Application.Model.Messages;

public class MessageDto
{
    public string Content { get; set; }

    public AttachmentDto[]? Attachments { get; init; }
    public FileUpload[]? Files { get; init; }
    public Embed[]? Embeds { get; set; }
    public Reaction[]? Reactions { get; init; }
    public Sticker[]? Stickers { get; init; }
    public string? Nonce { get; init; }
    public MessageReference? MessageReference { get; set; }

    public MessageFlags Flags { get; set; }
    public MessageType Type { get; init; }

    public bool IsPinned { get; init; }
    public bool IsTTS { get; init; }
}