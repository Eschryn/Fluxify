namespace Fluxify.Application.Entities.Messages;

[Flags]
public enum AttachmentFlags : uint
{
    IsSpoiler = 8,
    ContainsExplicitMedia = 16,
    IsAnimated = 32,
}