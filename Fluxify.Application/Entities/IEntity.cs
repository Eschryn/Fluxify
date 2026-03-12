using Fluxify.Core.Types;

namespace Fluxify.Application.Entities;

public interface IEntity
{
    Snowflake Id { get; }
}