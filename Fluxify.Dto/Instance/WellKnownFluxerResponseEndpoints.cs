namespace Fluxify.Dto.Instance;

public record WellKnownFluxerResponseEndpoints(
    Uri Api,
    Uri ApiClient,
    Uri ApiPublic,
    Uri Gateway,
    Uri Media,
    Uri StaticCdn,
    Uri Marketing,
    Uri Admin,
    Uri Invite,
    Uri Gift,
    Uri Webapp
);