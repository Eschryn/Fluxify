using System.Text.Json.Serialization;

namespace Fluxify.Dto.Channels.Text.Messages.Embeds;

[JsonConverter(typeof(JsonStringEnumConverter<MessageEmbedType>))]
public enum MessageEmbedType
{
    Rich,
    Image,
    Video,
    Gifv,
    Article,
    Link
}