using System.Text.Json.Serialization;

namespace Fluxify.Dto.Common;

[JsonConverter(typeof(MediaHashConverter))]
public readonly struct MediaHash(string hash)
{
    public string Hash { get; } = hash;
}