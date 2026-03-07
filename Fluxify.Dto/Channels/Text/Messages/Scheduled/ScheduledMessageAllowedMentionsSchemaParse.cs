using System.Text.Json.Serialization;

namespace Fluxify.Dto.Channels.Text.Messages.Scheduled;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ScheduledMessageAllowedMentionsSchemaParse
{
    Users,
    Roles,
    Everyone,
}