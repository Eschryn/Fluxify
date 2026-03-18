namespace Fluxify.Dto.Packs;

public record PackUpdateRequest(
    string? Description,
    string? Name
);