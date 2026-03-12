using Fluxify.Core.Types;

namespace Fluxify.Application.Entities.Guilds;

public class Sticker
{
    public string Name { get; set; }
    public bool IsAnimated { get; set; }
    public Snowflake? Id { get; set; }
}