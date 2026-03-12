using Fluxify.Application.Entities.Guilds;
using Fluxify.Core.Types;

namespace Fluxify.Application.Entities.Channels;

public class GuildLinkChannel(FluxerApplication fluxerApplication) : IGuildChannel
{
    public Snowflake Id { get; init; }
    public string Name { get; init; }
    public string? Url { get; init; }
    public Snowflake GuildId { get; init; }
    public GuildCategory? Parent { get; init; }
    public Guild? Guild { get; }
    public int? Position { get; init; }
    public PermissionOverwrite[]? Overwrites { get; init; }
}