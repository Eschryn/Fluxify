using Fluxify.Application.Entities.Guilds;
using Fluxify.Core.Types;

namespace Fluxify.Application.Entities.Channels;

public class GuildCategory(FluxerApplication fluxerApplication) : IGuildChannel
{
    public Snowflake Id { get; }
    public string Name { get; }
    public Snowflake GuildId { get; }
    public Guild? Guild { get; }
    public int? Position { get; init; }
    public PermissionOverwrite[]? Overwrites { get; init; }
}