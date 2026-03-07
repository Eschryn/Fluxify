namespace Fluxify.Dto.Gateway;

public record SessionStartLimits(
    long MaxConcurrency,
    long Remaining,
    long ResetAfter,
    long Total
);