using Fluxify.Core.Types;
using Fluxify.Dto.Channels.Text.Messages;
using Fluxify.Dto.Channels.Text.Messages.Attachments;
using Fluxify.Dto.Channels.Text.Messages.Embeds;
using Fluxify.Dto.Channels.Text.Messages.Scheduled;
using Fluxify.Dto.Uploads;

namespace Fluxify.Dto.Users.ScheduledMessages;

public record ScheduledMessageResponseSchemaPayload(
    AllowedMentionsSchema? AllowedMentions,
    MessageAttachmentResponse[]? Attachments,
    FileUpload[]? Files,
    string? Content,
    MessageEmbedResponse[]? Embeds,
    Snowflake? FavoriteMemeId,
    MessageFlags? Flags,
    ScheduledMessageReferenceSchema MessageReference,
    Snowflake? Nonce,
    Snowflake[]? StickerIds,
    MessageStickerResponse[]? Stickers,
    bool? Tts
) : MultipartDto(Files);