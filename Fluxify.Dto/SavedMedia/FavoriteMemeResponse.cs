using System.Net.Mime;
using Fluxify.Core.Types;

namespace Fluxify.Dto.SavedMedia;

public record FavoriteMemeResponse(
    string? AltText,
    Snowflake? AttachmentId,
    string? ContentHash,
    ContentType ContentType,
    double? Duration,
    string Filename,
    int? Height,
    int? Width,
    Snowflake Id,
    bool? IsGifv,
    string? KlipySlug,
    string Name,
    long Size,
    string[] Tags,
    string? TenorSlugId,
    string Url,
    Snowflake UserId
);