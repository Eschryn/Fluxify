using Fluxify.Application.Entities.Guilds;
using Fluxify.Core.Types;

namespace Fluxify.Application.Entities;

public interface IGuildScopedEntity
{
    public Snowflake GuildId { get; }
    public Guild? Guild { get; }
}