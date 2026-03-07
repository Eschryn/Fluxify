namespace Fluxify.Dto.SavedMedia;

public record UpdateFavoriteMemeBodySchema(
    string? AltText,
    string? Name,
    string[]? Tags
);