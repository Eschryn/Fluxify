using Fluxify.Core;
using Fluxify.Dto.Channels.Text.Messages;
using Fluxify.Dto.Channels.Text.Messages.Embeds;
using Fluxify.Dto.Channels.Text.Messages.Scheduled;

namespace Fluxify.Dto.Users.ScheduledMessages;

public record ScheduledMessageResponseSchemaPayload(
    ScheduledMessageAllowedMentionsSchema? AllowedMentions,
    Message[]? Attachments,
    string? Content,
    MessageEmbedResponse[]? Embeds,
    Snowflake? FavoriteMemeId,
    MessageFlags? Flags,
    ScheduledMessageReferenceSchema MessageReference,
    Snowflake? Nonce,
    Snowflake[]? StickerIds,
    MessageStickerResponse[]? Stickers,
    bool? Tts
);