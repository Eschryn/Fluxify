using System.Text.Json.Serialization;

namespace Fluxify.Dto.Channels.Text.Messages.Embeds;

[JsonConverter(typeof(JsonStringEnumConverter<MessageEmbedType>))]
public enum MessageEmbedType
{
    Image,
    Video,
    Sound,
    Article
}