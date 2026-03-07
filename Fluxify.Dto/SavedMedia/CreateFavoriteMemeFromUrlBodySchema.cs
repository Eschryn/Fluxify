namespace Fluxify.Dto.SavedMedia;

public record CreateFavoriteMemeFromUrlBodySchema(
    string? AltText,
    string? KlipySlug,
    string? Name,
    string[]? Tags,
    string? TenorSlugId,
    string Url
);