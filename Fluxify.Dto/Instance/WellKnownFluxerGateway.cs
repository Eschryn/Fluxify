namespace Fluxify.Dto.Instance;

public record WellKnownFluxerGateway(
    ulong SessionRetryMinMs,
    ulong SessionRetryMaxMs,
    ulong SessionRetryJitterMs
);