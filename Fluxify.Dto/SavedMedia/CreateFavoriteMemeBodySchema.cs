using Fluxify.Core.Types;

namespace Fluxify.Dto.SavedMedia;

public record CreateFavoriteMemeBodySchema(
    string? AltText,
    Snowflake? AttachmentId,
    long? EmbedIndex,
    string Name,
    string[]? Tags
);