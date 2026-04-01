// Copyright 2026 Fluxify Contributors
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Buffers;
using System.Buffers.Text;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Fluxify.Dto.Common;

public class Base64ImageConverter : JsonConverter<Base64Image>
{
    public override Base64Image Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var mimeType = "text/plain";
        string? charset = null;
        var dataUri = reader.GetString();
        var rest = dataUri.AsSpan();
        var indexOfAny = rest.IndexOfAny([';']);
        var firstPart = rest[5..indexOfAny];
        
        rest = rest[(indexOfAny + 1)..];

        if (!firstPart.IsEmpty)
        {
            mimeType = firstPart.ToString();
        }
        else
        {
            charset = "US-ASCII";
        }
        
        indexOfAny = rest.IndexOfAny([';']);
        if (indexOfAny != -1)
        {
            firstPart = rest[..indexOfAny];
            if (firstPart.StartsWith("charset="))
            {
                charset = firstPart[8..].ToString();
            }
            rest = rest[(indexOfAny + 1)..];
        }

        if (!rest.StartsWith("base64"))
        {
            throw new JsonException("Invalid base64 image format");
        }
        
        var data = charset?.ToLowerInvariant() switch
        {
            "utf-8" => Base64Url.DecodeFromUtf8(MemoryMarshal.AsBytes(rest[7..])),
            _ => Base64Url.DecodeFromChars(rest[7..])
        };
        
        return new Base64Image(data, mimeType, charset);
    }

    private static readonly CompositeFormat CompositeFormat = CompositeFormat.Parse("data:{0};charset={1};base64,");
    public override void Write(Utf8JsonWriter writer, Base64Image value, JsonSerializerOptions options)
    {
        var length = Base64Url.GetEncodedLength(value.Data.Length);
        var buffer = ArrayPool<byte>.Shared.Rent(length);
        var dataUriLength = 5 + value.MimeType.Length + 9 + (value.Charset?.Length ?? 5) + 8 + length;
        var stringBuilder = new StringBuilder(dataUriLength);
        stringBuilder.AppendFormat(
            CultureInfo.InvariantCulture,
            CompositeFormat,
            value.MimeType,
            "UTF-8"
        );

        var written = Base64Url.EncodeToUtf8(value.Data, buffer);
        stringBuilder.Append(buffer[..written]);
        
        writer.WriteStringValue(stringBuilder.ToString());
    }
}