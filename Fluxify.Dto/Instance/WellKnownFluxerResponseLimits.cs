namespace Fluxify.Dto.Instance;

public record WellKnownFluxerResponseLimits(
    string DefaultsHash,
    WellKnownFluxerResponseLimitsRule[] Rules,
    string[] TraitDefinitions,
    int Version
);