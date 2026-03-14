using Fluxify.Dto.Channels.Text.Messages.Attachments;
using Fluxify.Dto.Channels.Text.Messages.Embeds;

namespace Fluxify.Dto.Channels.Text.Messages;

public record UpdateMessageRequest(
    string? Content = null,
    MessageAttachmentRequest[]? Attachments = null,
    MessageEmbedResponse[]? Embeds = null,
    AllowedMentionsSchema? AllowedMentions = null, 
    MessageFlags? Flags = null
);