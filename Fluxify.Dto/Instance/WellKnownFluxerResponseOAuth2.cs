namespace Fluxify.Dto.Instance;

public record WellKnownFluxerResponseOAuth2(
    string AuthorizationEndpoint,
    string[] ScopesSupported,
    string TokenEndpoint,
    string UserinfoEndpoint
);