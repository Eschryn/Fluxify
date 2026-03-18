namespace Fluxify.Dto.Packs;

public record PackCreateRequest(
    string Name,
    string? Description
);