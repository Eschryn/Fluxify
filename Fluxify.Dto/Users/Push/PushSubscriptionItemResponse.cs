using Fluxify.Core.Types;

namespace Fluxify.Dto.Users.Push;

public record PushSubscriptionItemResponse(string SubscriptionId, string? UserAgent);