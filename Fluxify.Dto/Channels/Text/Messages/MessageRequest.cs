using Fluxify.Core.Types;
using Fluxify.Dto.Channels.Text.Messages.Embeds;
using Fluxify.Dto.Uploads;

namespace Fluxify.Dto.Channels.Text.Messages;

public record MessageRequest(
    string? Content = null,
    FileUpload[]? Files = null,
    SendMessageAttachmentRequest[]? Attachments = null,
    MessageEmbedResponse[]? Embeds = null,
    MessageReferenceResponse? MessageReference = null,
    string? Nonce = null,
    Snowflake[]? Stickers = null,
    bool? Tts = null
) : MultipartDto(Files);