using Fluxify.Core;
using Fluxify.Dto.Channels.Text.Messages.Embeds;

namespace Fluxify.Dto.Channels.Text.Messages;

public record MessageSnapshotResponse(
    MessageAttachmentResponse[]? Attachments,
    string? Content,
    DateTimeOffset? EditedTimestamp,
    MessageEmbedResponse[]? Embeds,
    MessageFlags Flags,
    Snowflake[]? MentionRoles,
    Snowflake[]? MentionUsers,
    MessageStickerResponse[]? Stickers,
    DateTimeOffset Timestamp,
    MessageType Type
);