using Fluxify.Core.Types;
using Fluxify.Dto.Channels.Text.Messages.Attachments;
using Fluxify.Dto.Channels.Text.Messages.Embeds;

namespace Fluxify.Dto.Channels.Text.Messages;

public record MessageSnapshotResponse(
    string? Content,
    DateTimeOffset Timestamp,
    DateTimeOffset? EditedTimestamp,
    Snowflake[]? Mentions,
    Snowflake[]? MentionRoles,
    MessageEmbedResponse[]? Embeds,
    MessageAttachmentResponse[]? Attachments,
    MessageStickerResponse[]? Stickers,
    MessageType Type,
    MessageFlags Flags
);