using Fluxify.Application.Entities.Messages;
using Fluxify.Application.Model.Messages.Embeds;
using Fluxify.Core.Types;
using Fluxify.Dto.Uploads;

namespace Fluxify.Application.Model.Messages;

public class MessageDto
{
    public string? Content { get; set; }
    public List<Attachment>? Attachments { get; set; }
    public List<Embed>? Embeds { get; set; }
    public AllowedMentions? AllowedMentions { get; set; }
    public MessageReference? MessageReference { get; set; }
    public MessageFlags? Flags { get; set; }
    public string? Nonce { get; init; }
    public Snowflake? FavoriteMemeId { get; set; }
    public List<Snowflake>? Stickers { get; set; }
    public bool Tts { get; set; }
    public List<FileUpload>? Files { get; set; }
}