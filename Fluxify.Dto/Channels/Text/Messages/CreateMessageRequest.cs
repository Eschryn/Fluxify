using System.ComponentModel.DataAnnotations;
using Fluxify.Core.Types;
using Fluxify.Dto.Channels.Text.Messages.Attachments;
using Fluxify.Dto.Channels.Text.Messages.Embeds;
using Fluxify.Dto.Uploads;

namespace Fluxify.Dto.Channels.Text.Messages;

public record CreateMessageRequest(
    string? Content = null,
    MessageAttachmentRequest[]? Attachments = null,
    MessageEmbedResponse[]? Embeds = null,
    AllowedMentionsSchema? AllowedMentions = null,
    MessageReferenceResponse? MessageReference = null,
    MessageFlags? Flags = null,
    [StringLength(32)] string? Nonce = null,
    Snowflake? FavoriteMemeId = null,
    [MaxLength(3)] Snowflake[]? Stickers = null,
    bool? Tts = null,
    FileUpload[]? Files = null
) : MultipartDto(Files);