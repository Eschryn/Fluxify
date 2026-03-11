using Fluxify.Core.Types;

namespace Fluxify.Dto.Channels.GroupDm;

public record CallRingBodySchema(
    Snowflake[] Recipents);