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

using System.Diagnostics.CodeAnalysis;
using Fluxify.Core.Types;

namespace Fluxify.Application.State.Ref;

// maybe implement INotifyPropertyChanged?
public class CacheRef<T>(Snowflake id, T? initialValue) : IEquatable<CacheRef<T>>, ICacheRef<T> where T : class, ICloneable<T>
{
    private T? _value = initialValue;
    
    public Snowflake Id { get; } = id;
    
    [MemberNotNullWhen(true, nameof(CacheRef<>.Value))]
    public T? Value => _value;
    
    internal void Swap(T newValue) => Interlocked.Exchange(ref _value, newValue);

    public bool Equals(CacheRef<T>? other) => Id.Equals(other?.Id) && EqualityComparer<T?>.Default.Equals(_value, other._value);
    public override bool Equals(object? obj) => obj is CacheRef<T> other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(_value, Id);
    public static bool operator ==(CacheRef<T> left, CacheRef<T> right) => left.Equals(right);
    public static bool operator !=(CacheRef<T> left, CacheRef<T> right) => !(left == right);
}