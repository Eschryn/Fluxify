namespace Fluxify.Dto.Channels.Text.Messages.Search;

public record MessageSearchResultsResponse(
    int HitsPerPage,
    MessageBaseResponse[] Messages,
    int Page,
    int Total
);