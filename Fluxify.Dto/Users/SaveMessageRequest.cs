using Fluxify.Core;

namespace Fluxify.Dto.Users;

public record SaveMessageRequest(
    Snowflake ChannelId,
    Snowflake MessageId
);