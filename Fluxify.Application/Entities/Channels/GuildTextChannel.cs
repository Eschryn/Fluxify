using Fluxify.Application.Entities.Guilds;
using Fluxify.Core.Types;

namespace Fluxify.Application.Entities.Channels;

public class GuildTextChannel(FluxerApplication fluxerApplication) : TextChannel(fluxerApplication), IGuildChannel
{
    public Guild? Guild { get; } 
    public Snowflake GuildId { get; init; }
    public string? Topic { get; init; }
    public int? Position { get; init; }
    public bool? Nsfw { get; init; }
    public GuildCategory? Parent { get; init; }
    public int? RateLimitPerUser { get; init; }
    public PermissionOverwrite[]? Overwrites { get; init; }
}