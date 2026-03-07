using System.Text.Json.Serialization;

namespace Fluxify.Dto.Channels.Text.Messages;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MessageContentType
{
    Image,
    Sound,
    Video,
    File,
    Sticker,
    Embed,
    Link,
    Poll,
    Snapshot
}