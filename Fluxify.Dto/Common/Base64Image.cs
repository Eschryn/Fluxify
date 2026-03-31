using System.Text.Json.Serialization;

namespace Fluxify.Dto.Common;

[JsonConverter(typeof(Base64ImageConverter))]
public readonly struct Base64Image(byte[] data, string mimeType, string? charset = null)
{
    public byte[] Data { get; } = data;
    public string MimeType { get; } = mimeType;
    public string? Charset { get; } = charset;
}