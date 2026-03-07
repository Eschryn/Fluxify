using System.Text.Json.Serialization;

namespace Fluxify.Dto.Users.ScheduledMessages;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ScheduledMessageStatus
{
    Pending,
    Invalid,
    Scheduled,
    Sent,
    Failed,
    Cancelled
}