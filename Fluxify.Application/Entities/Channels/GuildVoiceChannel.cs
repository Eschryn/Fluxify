using Fluxify.Application.Entities.Guilds;
using Fluxify.Core.Types;

namespace Fluxify.Application.Entities.Channels;

public class GuildVoiceChannel(FluxerApplication fluxerApplication) : IGuildChannel
{
    public Snowflake Id { get; init; }
    public required string Name { get; set; }
    public int Bitrate { get; init; }
    public int? UserLimit { get; init; }
    public Snowflake GuildId { get; init; }
    public Snowflake? RtcRegion { get; init; }
    public Guild? Guild { get; }
    public GuildCategory? Parent { get; init; }
    public int? Position { get; init; }
    public PermissionOverwrite[]? Overwrites { get; init; }
}