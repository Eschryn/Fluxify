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

[JsonConverter(typeof(MediaHashConverter))]
public readonly struct MediaHash(string hash) : IEquatable<MediaHash>, IEqualityOperators<MediaHash, MediaHash, bool>
{
    public string Hash { get; } = hash;

    public bool Equals(MediaHash other) => Hash == other.Hash;
    public override bool Equals(object? obj) => obj is MediaHash other && Equals(other);
    public override int GetHashCode() => Hash.GetHashCode();
    public static bool operator ==(MediaHash left, MediaHash right) => left.Hash == right.Hash;
    public static bool operator !=(MediaHash left, MediaHash right) => left.Hash != right.Hash;
}