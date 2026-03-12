namespace Fluxify.Dto.Channels.Text.Messages;

public record SendMessageAttachmentRequest(int Id, string? Description, string Filename, MessageAttachmentFlags Flags);