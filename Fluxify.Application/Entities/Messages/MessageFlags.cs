namespace Fluxify.Application.Entities.Messages;

public enum MessageFlags : uint
{
    SuppressEmbeds = 4,
    SuppressNotifications = 4096,
    VoiceMessage = 8192,
    CompactAttachments = 131072
}