namespace Fluxify.Dto.Users.Push;

public record PushSubscribeRequest(
    string Endpoint,
    PushSubscribeRequestKeys Keys,
    string? UserAgent
);