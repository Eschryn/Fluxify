using System.Text.Json.Serialization;

namespace Fluxify.Dto.Channels.Text.Messages.BulkDelete;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MessageShredStatusProgressResponseStatus
{
    InProgress,
    Completed,
    Failed
}