namespace Fluxify.Dto.Instance;

public record SsoStatusResponse(
    bool Enabled,
    bool Enforced,
    string? DisaplayName,
    string RedirectUri
);