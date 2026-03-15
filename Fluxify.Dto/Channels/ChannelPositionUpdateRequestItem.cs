using Fluxify.Core.Types;

namespace Fluxify.Dto.Channels;

public record ChannelPositionUpdateRequest(
    Snowflake Id,
    bool? LockPermissions,
    Snowflake? ParentId,
    long Position,
    Snowflake? PrecedingSiblingId
);