using Fluxify.Core;
using Fluxify.Dto.Channels.GroupDm;
using Fluxify.Dto.Channels.Text.Messages.Embeds;
using Fluxify.Dto.Users;

namespace Fluxify.Dto.Channels.Text.Messages;

public record Message(
    MessageAttachmentResponse[]? Attachments,
    UserResponse Author,
    MessageCallResponse? Call,
    Snowflake ChannelId,
    string Content,
    DateTimeOffset? EditedTimestamp,
    MessageEmbedResponse[]? Embeds,
    MessageFlags Flags,
    Snowflake Id,
    bool MentionEveryone,
    Snowflake[] MentionRoles,
    UserResponse[]? Mentions,
    MessageReferenceResponse? MessageReference,
    MessageSnapshotResponse[]? MessageSnapshots,
    string? Nonce,
    bool Pinned,
    MessageReactionResponse[]? Reactions,
    MessageBaseResponseSchema? ReferredMessage,
    MessageStickerResponse[]? Stickers,
    DateTimeOffset Timestamp,
    bool? Tts,
    MessageType Type,
    Snowflake? WebhookId
);