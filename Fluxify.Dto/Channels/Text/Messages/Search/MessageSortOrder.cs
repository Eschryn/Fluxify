using System.Text.Json.Serialization;

namespace Fluxify.Dto.Channels.Text.Messages.Search;

public enum MessageSortOrder
{
    [JsonStringEnumMemberName("asc")] Ascending,
    [JsonStringEnumMemberName("desc")] Descending
}