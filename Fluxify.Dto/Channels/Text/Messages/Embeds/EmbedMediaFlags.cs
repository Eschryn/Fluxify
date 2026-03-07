namespace Fluxify.Dto.Channels.Text.Messages.Embeds;

[Flags]
public enum EmbedMediaFlags : uint
{
    ContainsExplicitMedia = 16,
    IsAnimated = 32,
}