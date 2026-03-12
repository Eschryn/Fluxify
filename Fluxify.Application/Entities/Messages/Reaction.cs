using Fluxify.Application.Entities.Guilds;

namespace Fluxify.Application.Entities.Messages;

public class Reaction
{
    public Emoji Emoji { get; set; }
    public int Count { get; set; }
    public bool Me { get; set; }
}