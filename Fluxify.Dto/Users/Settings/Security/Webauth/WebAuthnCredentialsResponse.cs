namespace Fluxify.Dto.Users.Settings.Security.Webauth;

public record WebAuthnCredentialsResponse(string Id, string Name, DateTimeOffset CreatedAt, DateTimeOffset? LastUsedAt);