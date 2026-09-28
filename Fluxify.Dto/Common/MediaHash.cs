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

using System.Numerics;
using System.Text.Json.Serialization;

namespace Fluxify.Dto.Common;

/// <summary>
/// Represents a media (image/video/sound) stored in the media proxy.
/// </summary>
/// <param name="hash">The access hash for the media.</param>
/// <remarks>
/// To access the media one will need to request it using the hash from the media proxy api.
/// </remarks>
/// <seealso href="https://docs.fluxer.app/media-proxy/overview/"/>
[JsonConverter(typeof(MediaHashConverter))]
public readonly struct MediaHash(string hash) : IEquatable<MediaHash>, IEqualityOperators<MediaHash, MediaHash, bool>
{
    /// <summary>
    /// The access hash for the media in the media proxy.
    /// </summary>
    public string Hash { get; } = hash;
    
    /// <inheritdoc />
    public bool Equals(MediaHash other) => Hash == other.Hash;

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is MediaHash other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => Hash.GetHashCode();

    /// <inheritdoc />
    public static bool operator ==(MediaHash left, MediaHash right) => left.Hash == right.Hash;

    /// <inheritdoc />
    public static bool operator !=(MediaHash left, MediaHash right) => left.Hash != right.Hash;
}