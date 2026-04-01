namespace Fluxify.Dto.Instance;

public record WellKnownFluxerResponseAttachments(
    bool MultipartEnabled,
    int MultipartThresholdBytes,
    int MultipartChunkSizeBytes,
    int MultipartMaxConcurrency
);