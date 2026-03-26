using Fluxify.Application.Entities.Channels;

namespace Fluxify.Application.Entities.Users;

public class VoiceState : IVoiceState
{
    public required GuildVoiceChannel VoiceChannel { get; init; }
    public bool Mute { get; internal set; }
    public bool Deaf { get; internal set; }
    public bool? SelfStream { get; internal set; }
    public bool SelfDeaf { get; internal set; }
    public bool SelfMute { get; internal set; }
    public bool? SelfVideo { get; internal set; }
    public bool? IsMobile { get; internal set; }
    public string? SessionId { get; internal set; }
    public string ConnectionId { get; internal set; } = string.Empty;
    public int Version { get; internal set; }
    public string[]? ViewerStreamKeys { get; internal set; }
}