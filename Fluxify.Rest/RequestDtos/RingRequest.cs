using Fluxify.Core.Types;

namespace Fluxify.Rest.RequestDtos;

public record RingRequest(Snowflake[] Recipients);