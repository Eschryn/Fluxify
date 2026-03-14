using System.Text.Json.Serialization;

namespace Fluxify.Dto.Channels.Text.Messages;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AllowedMentionsSchemaParse
{
    Users,
    Roles,
    Everyone,
}