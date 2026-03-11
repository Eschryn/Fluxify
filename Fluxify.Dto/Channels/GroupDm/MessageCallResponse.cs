using Fluxify.Core.Types;

namespace Fluxify.Dto.Channels.GroupDm;

public record MessageCallResponse(
    DateTimeOffset? EndedTimestamp,
    Snowflake[] Participants
);