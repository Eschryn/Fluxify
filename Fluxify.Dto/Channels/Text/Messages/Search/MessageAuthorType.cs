using System.Text.Json.Serialization;

namespace Fluxify.Dto.Channels.Text.Messages.Search;

[JsonConverter(typeof(JsonStringEnumConverter<MessageContentType>))]
public enum MessageAuthorType
{
    User,
    Bot,
    Webhook
}