namespace Fluxify.Dto.Channels.GroupDm;

public record CallEligibilityResponse(
    bool Ringable,
    bool Silent
);