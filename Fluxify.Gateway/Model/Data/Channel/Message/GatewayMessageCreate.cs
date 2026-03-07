using Fluxify.Core;
using Fluxify.Dto.Channels.GroupDm;
using Fluxify.Dto.Channels.Text.Messages;
using Fluxify.Dto.Channels.Text.Messages.Embeds;
using Fluxify.Dto.Users;

namespace Fluxify.Gateway.Model.Dto;

public record GatewayMessageCreate(
    MessageAttachmentResponse[]? Attachments,
    User Author,
    MessageCallResponse? Call,
    Snowflake ChannelId,
    string Content,
    DateTimeOffset? EditedTimestamp,
    MessageEmbedResponse[]? Embeds,
    MessageFlags Flags,
    Snowflake Id,
    bool MentionEveryone,
    Snowflake[] MentionRoles,
    User[]? Mentions,
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
    Snowflake? WebhookId,
    int ChannelType
) : Message(Attachments, Author, Call, ChannelId, Content, EditedTimestamp, Embeds, Flags, Id, MentionEveryone,
    MentionRoles, Mentions, MessageReference, MessageSnapshots, Nonce, Pinned, Reactions, ReferredMessage, Stickers,
    Timestamp, Tts, Type, WebhookId);