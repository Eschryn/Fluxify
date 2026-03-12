using Fluxify.Application.Entities.Users;
using Fluxify.Core.Types;

namespace Fluxify.Application.Entities.Channels;

public class GroupDm(FluxerApplication fluxerApplication)
    : TextChannel(fluxerApplication)
{
    public string IconHash { get; init; }
    public Snowflake OwnerId { get; init; }
    public PartialUser[] Recipients { get; init; }
    public Dictionary<string, string> Nicks { get; init; }
}