using Fluxify.Application.Entities.Channels;

namespace Fluxify.Application.Entities.Users;

public interface IVoiceState
{
    public GuildVoiceChannel VoiceChannel { get; }
    public bool Mute { get; }
    public bool Deaf { get; }
}