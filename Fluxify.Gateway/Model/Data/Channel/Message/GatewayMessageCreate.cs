using Fluxify.Core.Types;
using Fluxify.Dto.Channels;
using Fluxify.Dto.Channels.GroupDm;
using Fluxify.Dto.Channels.Text.Messages;
using Fluxify.Dto.Channels.Text.Messages.Attachments;
using Fluxify.Dto.Channels.Text.Messages.Embeds;
using Fluxify.Dto.Channels.Text.Messages.Reactions;
using Fluxify.Dto.Users;

namespace Fluxify.Gateway.Model.Data.Channel.Message;

public record GatewayMessageCreate(
    Snowflake Id,
    Snowflake ChannelId,
    UserPartialResponse Author,
    Snowflake? WebhookId,
    MessageType Type,
    MessageFlags Flags,
    string Content,
    DateTimeOffset Timestamp,
    DateTimeOffset? EditedTimestamp,
    bool Pinned,
    bool MentionEveryone,
    bool? Tts,
    UserPartialResponse[]? Mentions,
    Snowflake[]? MentionRoles,
    MessageEmbedResponse[]? Embeds,
    MessageAttachmentResponse[]? Attachments,
    MessageStickerResponse[]? Stickers,
    MessageReactionResponse[]? Reactions,
    MessageReferenceResponse? MessageReference,
    MessageSnapshotResponse[]? MessageSnapshots,
    string? Nonce,
    MessageCallResponse? Call,
    MessageBaseResponse? ReferencedMessage,
    ChannelType ChannelType
) : MessageResponse(
    Id,
    ChannelId,
    Author,
    WebhookId,
    Type,
    Flags,
    Content,
    Timestamp,
    EditedTimestamp,
    Pinned,
    MentionEveryone,
    Tts,
    Mentions,
    MentionRoles,
    Embeds,
    Attachments,
    Stickers,
    Reactions,
    MessageReference,
    MessageSnapshots,
    Nonce,
    Call,
    ReferencedMessage
);