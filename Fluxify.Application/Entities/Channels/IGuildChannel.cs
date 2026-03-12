namespace Fluxify.Application.Entities.Channels;

public interface IGuildChannel : IChannel, IGuildScopedEntity
{
    public int? Position { get; init; }
    public PermissionOverwrite[]? Overwrites { get; init; }
}