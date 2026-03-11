using Fluxify.Core.Types;

namespace Fluxify.Dto.Users.Push;

public record PushSubscriptionItemResponse(Snowflake SubscriptionId, string? UserAgent);