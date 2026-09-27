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
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace Fluxify.Core.Types;

/// <summary>
/// Represents an ID from Fluxer.
/// </summary>
/// <param name="value">Numerical representation of the ID.</param>
/// <remarks>
/// Fluxer IDs are composite IDs where the components are bitwise combined into one numerical representation.
/// <br/>
/// <br/>
/// The components are the following:
/// <list type="bullet">
/// <item>
/// Timestamp in Fluxer Epoch - 0 is 1st January 2015
/// </item>
/// <item>
/// Worker ID
/// </item>
/// <item>
/// Internal Process ID
/// </item>
/// <item>
/// Process snowflake count - value is incremented for each snowflake created by the process.
/// </item>
/// </list>
///
/// </remarks>
[Serializable]
[JsonConverter(typeof(SnowflakeConverter))]
[StructLayout(LayoutKind.Sequential)]
public readonly struct Snowflake(ulong value) : IConvertible,
    IComparable,
    IComparable<Snowflake>,
    IEquatable<Snowflake>,
    IMinMaxValue<Snowflake>,
    ISpanParsable<Snowflake>,
    IEqualityOperators<Snowflake, Snowflake, bool>,
    ISpanFormattable,
    IUtf8SpanFormattable
{
    private readonly ulong _value = value;

    /// <inheritdoc />
    public int CompareTo(Snowflake other) => _value.CompareTo(other._value);

    /// <inheritdoc />
    public bool Equals(Snowflake other) => _value == other._value;

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is Snowflake other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => _value.GetHashCode();

    /// <inheritdoc />
    public string ToString(string? format, IFormatProvider? formatProvider) => _value.ToString(format, formatProvider);

    /// <inheritdoc />
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format,
        IFormatProvider? provider) => _value.TryFormat(destination, out charsWritten, format, provider);

    /// <inheritdoc />
    public override string ToString() => _value.ToString();

    /// <inheritdoc />
    public int CompareTo(object? obj) => obj is Snowflake other
        ? CompareTo(other)
        : throw new ArgumentException("obj must be Snowflake");

    /// <inheritdoc />
    public bool TryFormat(
        Span<byte> destination,
        out int bytesWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
        => _value.TryFormat(destination, out bytesWritten, format, provider);

    /// <inheritdoc />
    public TypeCode GetTypeCode() => TypeCode.UInt64;

    /// <inheritdoc />
    public bool ToBoolean(IFormatProvider? provider) => Convert.ToBoolean(_value);

    /// <inheritdoc />
    public byte ToByte(IFormatProvider? provider) => Convert.ToByte(_value);

    /// <inheritdoc />
    public char ToChar(IFormatProvider? provider) => Convert.ToChar(_value);

    /// <inheritdoc />
    public DateTime ToDateTime(IFormatProvider? provider) => Convert.ToDateTime(_value);

    /// <inheritdoc />
    public decimal ToDecimal(IFormatProvider? provider) => Convert.ToDecimal(_value);

    /// <inheritdoc />
    public double ToDouble(IFormatProvider? provider) => Convert.ToDouble(_value);

    /// <inheritdoc />
    public short ToInt16(IFormatProvider? provider) => Convert.ToInt16(_value);

    /// <inheritdoc />
    public int ToInt32(IFormatProvider? provider) => Convert.ToInt32(_value);

    /// <inheritdoc />
    public long ToInt64(IFormatProvider? provider) => Convert.ToInt64(_value);

    /// <inheritdoc />
    public sbyte ToSByte(IFormatProvider? provider) => Convert.ToSByte(_value);

    /// <inheritdoc />
    public float ToSingle(IFormatProvider? provider) => Convert.ToSingle(_value);

    /// <inheritdoc />
    public string ToString(IFormatProvider? provider) => Convert.ToString(_value, provider);

    /// <inheritdoc />
    public object ToType(Type conversionType, IFormatProvider? provider) =>
        Convert.ChangeType(_value, conversionType, provider);

    /// <inheritdoc />
    public ushort ToUInt16(IFormatProvider? provider) => Convert.ToUInt16(_value);

    /// <inheritdoc />
    public uint ToUInt32(IFormatProvider? provider) => Convert.ToUInt32(_value);

    /// <inheritdoc />
    public ulong ToUInt64(IFormatProvider? provider) => Convert.ToUInt64(_value);

    /// <inheritdoc />
    public static Snowflake MaxValue { get; } = new(ulong.MaxValue);

    /// <inheritdoc />
    public static Snowflake MinValue { get; } = new(ulong.MinValue);

    /// <inheritdoc />
    public static bool operator ==(Snowflake left, Snowflake right) => left.Equals(right);
    /// <inheritdoc />
    public static bool operator !=(Snowflake left, Snowflake right) => !(left == right);
    
    /// <summary>
    /// Compares two values to determine if left is bigger than right.
    /// </summary>
    /// <param name="left">The value to compare with right.</param>
    /// <param name="right">The value to compare with left.</param>
    /// <returns>true if left is bigger than right, otherwise false.</returns>
    public static bool operator >(Snowflake left, Snowflake right) => !(left == right);
    
    /// <summary>
    /// Compares two values to determine if left is smaller than right.
    /// </summary>
    /// <param name="left">The value to compare with right.</param>
    /// <param name="right">The value to compare with left.</param>
    /// <returns>true if left is smaller than right, otherwise false.</returns>
    public static bool operator <(Snowflake left, Snowflake right) => !(left == right);
    
    /// <summary>
    /// Converts numeric <see cref="ulong"/> representation to <see cref="Snowflake"/>.
    /// </summary>
    /// <param name="u64">The numeric representation of the snowflake.</param>
    /// <returns>The snowflake represented by <paramref name="u64"/>.</returns>
    public static implicit operator Snowflake(ulong u64) => new(u64);
    
    /// <summary>
    /// Converts numeric <see cref="long"/> representation to <see cref="Snowflake"/>.
    /// </summary>
    /// <param name="i64">The numeric representation of the snowflake.</param>
    /// <returns>The snowflake represented by <paramref name="i64"/>.</returns>
    public static implicit operator Snowflake(long i64) => new((ulong)i64);
    
    /// <summary>
    /// Converts the <see cref="Snowflake"/> into numeric <see cref="ulong"/> representation.
    /// </summary>
    /// <param name="s">The snowflake to be converted.</param>
    /// <returns>The numeric <see cref="ulong"/> representation of <paramref name="s"/>.</returns>
    public static implicit operator ulong(Snowflake s) => s._value;
    
    /// <summary>
    /// Converts the <see cref="Snowflake"/> into numeric <see cref="long"/> representation.
    /// </summary>
    /// <param name="s">The snowflake to be converted.</param>
    /// <returns>The numeric <see cref="long"/> representation of <paramref name="s"/>.</returns>
    public static implicit operator long(Snowflake s) => (long)s._value;
    
    /// <inheritdoc />
    public static Snowflake Parse(string s, IFormatProvider? provider) => new(ulong.Parse(s, provider));

    /// <inheritdoc />
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Snowflake result)
    {
        if (ulong.TryParse(s, provider, out var value))
        {
            result = new(value);
            return true;
        }

        result = default;
        return false;
    }

    /// <inheritdoc />
    public static Snowflake Parse(ReadOnlySpan<char> s, IFormatProvider? provider) => new(ulong.Parse(s, provider));

    /// <inheritdoc />
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Snowflake result)
    {
        if (ulong.TryParse(s, provider, out var value))
        {
            result = new Snowflake(value);
            return true;
        }

        result = default;
        return false;
    }

    /// <summary>
    /// Creates a new <see cref="Snowflake"/> using <see cref="SnowflakeGenerator.Default"/>.
    /// </summary>
    /// <returns>The generated <see cref="Snowflake"/></returns>
    /// <remarks>
    /// For <see cref="SnowflakeGenerator.Default"/> Worker ID and Internal Process ID will be 0.
    /// </remarks>
    public static Snowflake Create() => SnowflakeGenerator.Default.Create();
}