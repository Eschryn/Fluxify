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

using System.Text.Json.Serialization;

namespace Fluxify.Dto.Common;

/// <summary>
/// Base64 encoded image data.
/// </summary>
/// <param name="data">The contents of the image file.</param>
/// <param name="mimeType">The mime type of the image file.</param>
/// <param name="charset">The encoding for base64 encoded image file.</param>
[JsonConverter(typeof(Base64ImageConverter))]
public readonly struct Base64Image(byte[] data, string mimeType, string? charset = null)
{
    /// <summary>
    /// The contents of the image file.
    /// </summary>
    public byte[] Data { get; } = data;

    /// <summary>
    /// The mime type of the image file.
    /// </summary>
    public string MimeType { get; } = mimeType;
    
    /// <summary>
    /// The encoding for base64 encoded image file.
    /// </summary>
    public string? Charset { get; } = charset;
}