namespace Fluxify.Dto.Channels.Text.Messages;

[Flags]
public enum MessageAttachmentFlags : uint
{
    IsSpoiler = 8,
    ContainsExplicitMedia = 16,
    IsAnimated = 32,
}