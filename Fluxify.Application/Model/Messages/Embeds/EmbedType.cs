using System.Text.Json.Serialization;

namespace Fluxify.Application.Model.Messages.Embeds;

[JsonConverter( typeof(JsonStringEnumConverter<EmbedType>))]
public enum EmbedType
{
    Rich,
    Image,
    Video,
    Gifv,
    Article,
    Link
}