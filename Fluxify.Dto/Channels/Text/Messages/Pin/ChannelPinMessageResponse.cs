using Fluxify.Core.Types;
using Fluxify.Dto.Channels.GroupDm;
using Fluxify.Dto.Channels.Text.Messages.Embeds;
using Fluxify.Dto.Users;

namespace Fluxify.Dto.Channels.Text.Messages.Pin;

public record ChannelPinMessageResponse(
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
    MessageReferenceResponse? ReferenceResponse,
    MessageSnapshotResponse[]? MessageSnapshots,
    string? Nonce,
    bool Pinned,
    MessageStickerResponse[]? Stickers,
    DateTimeOffset Timestamp,
    bool? Tts,
    MessageType Type,
    Snowflake? WebhookId
);