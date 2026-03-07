using Fluxify.Core;

namespace Fluxify.Dto.Channels;

record ChannelPositionUpdateRequestItem(
    Snowflake Id,
    bool? LockPermissions,
    Snowflake? ParentId,
    long? Position,
    Snowflake? PrecedingSiblingId
);