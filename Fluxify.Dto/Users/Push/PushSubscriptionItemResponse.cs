using Fluxify.Core;

namespace Fluxify.Dto.Users.Push;

public record PushSubscriptionItemResponse(Snowflake SubscriptionId, string? UserAgent);