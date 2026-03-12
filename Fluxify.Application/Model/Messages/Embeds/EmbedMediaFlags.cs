namespace Fluxify.Application.Model.Messages.Embeds;

[Flags]
public enum EmbedMediaFlags : uint
{
    ContainsExplicitMedia = 16,
    IsAnimated = 32,
}