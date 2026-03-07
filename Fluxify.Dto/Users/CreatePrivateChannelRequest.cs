using Fluxify.Core;

namespace Fluxify.Dto.Users;

public record CreatePrivateChannelRequest(
    Snowflake RecipientId,
    Snowflake[] Recipients
);