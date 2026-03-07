namespace Fluxify.Dto.Users;

public record UserSessionResponse(
    DateTimeOffset ApproxLastUsedAt,
    string ClientIp,
    string? ClientIpReverse,
    string? ClientLocation,
    string? ClientOs,
    string? ClientPlatform,
    DateTimeOffset CreatedAt,
    string SessionIdHash
);