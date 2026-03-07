using Fluxify.Core;

namespace Fluxify.Dto.Channels.GroupDm;

public record CallRingBodySchema(
    Snowflake[] Recipents);