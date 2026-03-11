using Fluxify.Core.Types;

namespace Fluxify.Dto.Users;

public record SaveMessageRequest(
    Snowflake ChannelId,
    Snowflake MessageId
);