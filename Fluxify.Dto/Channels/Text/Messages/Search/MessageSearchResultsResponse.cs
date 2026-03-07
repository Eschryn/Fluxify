namespace Fluxify.Dto.Channels.Text.Messages.Search;

public record MessageSearchResultsResponse(
    int HitsPerPage,
    MessageBaseResponseSchema[] Messages,
    int Page,
    int Total
);