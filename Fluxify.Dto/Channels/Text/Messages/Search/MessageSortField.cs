using System.Text.Json.Serialization;

namespace Fluxify.Dto.Channels.Text.Messages.Search;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MessageSortField
{
    Timestamp,
    Relevance
}