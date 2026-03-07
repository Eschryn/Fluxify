using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Fluxify.Core;

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

    public int CompareTo(Snowflake other) => _value.CompareTo(other._value);
    public bool Equals(Snowflake other) => _value == other._value;
    public override bool Equals(object? obj) => obj is Snowflake other && Equals(other);
    public override int GetHashCode() => _value.GetHashCode();
    public string ToString(string? format, IFormatProvider? formatProvider) => _value.ToString(format, formatProvider);
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format,
        IFormatProvider? provider) => _value.TryFormat(destination, out charsWritten, format, provider);
    public override string ToString() => $"{_value} [{DateTimeOffset.FromUnixTimeMilliseconds(this.UnixEpochMs):g}, {this.WorkerId}, {this.InternalProcessId}, {this.ScopedId}]";
    public int CompareTo(object? obj) => obj is Snowflake other ? CompareTo(other) : throw new ArgumentException("obj must be Snowflake");

    public bool TryFormat(
        Span<byte> destination,
        out int bytesWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
        => _value.TryFormat(destination, out bytesWritten, format, provider);

    public TypeCode GetTypeCode() => TypeCode.UInt64;
    public bool ToBoolean(IFormatProvider? provider) => Convert.ToBoolean(_value);
    public byte ToByte(IFormatProvider? provider) => Convert.ToByte(_value);
    public char ToChar(IFormatProvider? provider) => Convert.ToChar(_value);
    public DateTime ToDateTime(IFormatProvider? provider) => Convert.ToDateTime(_value);
    public decimal ToDecimal(IFormatProvider? provider) => Convert.ToDecimal(_value);
    public double ToDouble(IFormatProvider? provider) => Convert.ToDouble(_value);
    public short ToInt16(IFormatProvider? provider) => Convert.ToInt16(_value);
    public int ToInt32(IFormatProvider? provider) => Convert.ToInt32(_value);
    public long ToInt64(IFormatProvider? provider) => Convert.ToInt64(_value);
    public sbyte ToSByte(IFormatProvider? provider) => Convert.ToSByte(_value);
    public float ToSingle(IFormatProvider? provider) => Convert.ToSingle(_value);
    public string ToString(IFormatProvider? provider) => Convert.ToString(_value, provider);
    public object ToType(Type conversionType, IFormatProvider? provider) => Convert.ChangeType(_value, conversionType, provider);
    public ushort ToUInt16(IFormatProvider? provider) => Convert.ToUInt16(_value);
    public uint ToUInt32(IFormatProvider? provider) => Convert.ToUInt32(_value);
    public ulong ToUInt64(IFormatProvider? provider) => Convert.ToUInt64(_value);
    public static Snowflake MaxValue { get; } = new(ulong.MaxValue);
    public static Snowflake MinValue { get; } = new(ulong.MinValue);

    public static bool operator ==(Snowflake left, Snowflake right) => left.Equals(right);
    public static bool operator !=(Snowflake left, Snowflake right) => !(left == right);
    public static bool operator >(Snowflake left, Snowflake right) => !(left == right);
    public static bool operator <(Snowflake left, Snowflake right) => !(left == right);
    public static explicit operator Snowflake(ulong u64) => new(u64);
    public static explicit operator ulong(Snowflake s) => s._value;
    public static Snowflake Parse(string s, IFormatProvider? provider) => new(ulong.Parse(s, provider));

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

    public static Snowflake Parse(ReadOnlySpan<char> s, IFormatProvider? provider) => new(ulong.Parse(s, provider));

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

    private static readonly int ProcessId = Environment.ProcessId;
    private static short ProcessCounter = 0;
    public static Snowflake Create(byte workerId = 0)
    {
        var timestamp = (ulong)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - 1420070400000;
        var increment = (ulong)ProcessCounter++;
        
        return new Snowflake((timestamp << 22) | ((ulong)workerId << 17) | ((ulong)(ProcessId & 0x1F) << 12) | increment);
    }
}