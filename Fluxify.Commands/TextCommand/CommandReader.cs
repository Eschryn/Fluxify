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
using System.Collections.Frozen;
using System.Runtime.InteropServices;
using Fluxify.Commands.Exceptions;

namespace Fluxify.Commands.TextCommand;

public class CommandReader(CommandTokenizer tokenizer)
{
    private static readonly SearchValues<char> StringDelimiters = SearchValues.Create("\"'<>");
    private static readonly SearchValues<char> TopLevelDelimiters = SearchValues.Create("<\"'");

    private static readonly Dictionary<Type, Func<ReadOnlyMemory<char>, object?>> Parsers = new()
    {
        { typeof(string), memory => memory.ToString() },
        { typeof(sbyte), memory => sbyte.Parse(memory.Span) },
        { typeof(byte), memory => byte.Parse(memory.Span) },
        { typeof(short), memory => short.Parse(memory.Span) },
        { typeof(ushort), memory => ushort.Parse(memory.Span) },
        { typeof(int), memory => int.Parse(memory.Span) },
        { typeof(uint), memory => uint.Parse(memory.Span) },
        { typeof(long), memory => long.Parse(memory.Span) },
        { typeof(ulong), memory => ulong.Parse(memory.Span) },
        { typeof(nint), memory => nint.Parse(memory.Span) },
        { typeof(nuint), memory => nuint.Parse(memory.Span) },
        { typeof(float), memory => float.Parse(memory.Span) },
        { typeof(double), memory => double.Parse(memory.Span) },
        { typeof(decimal), memory => decimal.Parse(memory.Span) },
        { typeof(bool), memory => bool.Parse(memory.Span) },
        { typeof(char), memory => memory.Span[0] },
        { typeof(char[]), memory => memory.Span.ToArray() },
        { typeof(ReadOnlyMemory<char>), memory => memory },
        {
            typeof(Uri),
            c => Uri.TryCreate(
                c.ToString().TrimStart('<').TrimEnd('>'),
                UriKind.RelativeOrAbsolute,
                out var uri
            )
                ? uri
                : null
        }
    };

    private static FrozenDictionary<Type, Func<ReadOnlyMemory<char>, object?>> _frozenParsers =
        Parsers.ToFrozenDictionary();

    private object? _lastFailedRead;

    public T GetNext<T>()
        => (TryGetNext<T>(out var next)
            ? next
            : default) ?? throw new CommandException(
            $"Invalid arguments provided. Expected: {typeof(T).Name}, got {next?.GetType().Name}.");

    public bool TryGetNext<T>(out T? result)
    {
        var next = _lastFailedRead ?? GetNext(true);
        _lastFailedRead = null;

        switch (next)
        {
            case T value:
                result = value;
                return true;
            case ReadOnlyMemory<char> memory
                when TryParseMemory(memory, typeof(T), out var parsedObject)
                     && parsedObject is T obj:
                result = obj;
                return true;
            default:
                result = default;
                _lastFailedRead = next;
                return false;
        }
    }

    private bool TryParseMemory(ReadOnlyMemory<char> memory, Type type, out object? result)
    {
        result = null;

        try
        {
            if (_frozenParsers.TryGetValue(type, out var parser)
                && parser(memory) is { } obj)
            {
                result = obj;
            }
            else if (type == typeof(ReadOnlyMemory<char>))
                result = memory;
        }
        catch (FormatException)
        {
            throw new CommandException($"Invalid argument provided. Malformed {type.Name}.");
        }

        return result is not null;
    }

    public object GetNext(bool ignoreSpace = false)
    {
        var token = tokenizer.Until(TopLevelDelimiters);
        switch (token.Span)
        {
            case "<" when ParseMention() is { } mention:
                return mention;
            case "<": // url
                return ParseQuotedString(token.Span, '>', escapable: false).ToString().AsMemory();
            case "\"" or "'":
                return ParseQuotedString(token.Span)[1..^1].ToString().AsMemory();
            default:
                return token;
        }
    }

    private ReadOnlySpan<char> ParseQuotedString(ReadOnlySpan<char> startSpan, char? endChar = null,
        bool escapable = true)
    {
        endChar ??= startSpan[0];
        var totalLength = startSpan.Length;
        var ignoreNext = false;
        var terminated = false;

        while (tokenizer.HasMore)
        {
            var readOnlyMemory = tokenizer.Until(StringDelimiters);
            var lastPos = readOnlyMemory.Length - 1;
            totalLength += readOnlyMemory.Length;
            if (readOnlyMemory.Span[lastPos] == endChar && !ignoreNext)
            {
                terminated = true;
                break;
            }

            ignoreNext = readOnlyMemory.Span[lastPos] == '\\' && escapable;
        }

        if (!terminated)
        {
            throw new CommandParameterFormatException("Unterminated quoted string.");
        }

        return MemoryMarshal.CreateReadOnlySpan(
            ref MemoryMarshal.GetReference(startSpan),
            totalLength
        );
    }

    private Mentionable? ParseMention()
    {
        var parsed = true;
        try
        {
            var type = tokenizer.Peek().Span;
            switch (type)
            {
                case "@":
                    tokenizer.ConsumeNext();
                    var snowflakeStr = tokenizer.Next();
                    switch (snowflakeStr.Span)
                    {
                        case "!":
                            snowflakeStr = tokenizer.Next();
                            return new Mentionable.User(ulong.Parse(snowflakeStr.Span));
                        case "&":
                            snowflakeStr = tokenizer.Next();
                            return new Mentionable.Role(ulong.Parse(snowflakeStr.Span));
                        default:
                            return new Mentionable.User(ulong.Parse(snowflakeStr.Span));
                    }
                case "#":
                    tokenizer.ConsumeNext();
                    snowflakeStr = tokenizer.Next();
                    return new Mentionable.Channel(ulong.Parse(snowflakeStr.Span));
                case ":":
                    tokenizer.ConsumeNext();
                    var name = tokenizer.Next().Span;
                    if (tokenizer.Next().Span is not ":")
                        throw new CommandParameterFormatException(
                            "Emoji name must be followed by a colon and an emoji id.s");

                    var emojiId = tokenizer.Next().Span;
                    return new Mentionable.Emoji(name.ToString(), ulong.Parse(emojiId));
                case "t":
                    tokenizer.ConsumeNext();
                    snowflakeStr = tokenizer.Next();
                    if (snowflakeStr.Span is not ":")
                        throw new CommandParameterFormatException("Not a valid datetime format.");

                    var dateTimeStr = tokenizer.Next().Span;
                    var dateTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(dateTimeStr));

                    if (tokenizer.Next().Span is not ":")
                        return new Mentionable.DateTime(dateTime, "R");

                    var format = tokenizer.Next().Span;
                    return new Mentionable.DateTime(dateTime, format.ToString());
                default:
                    parsed = false;
                    return null;
            }
        }
        finally
        {
            if (parsed && tokenizer.Next().Span is not ">")
                throw new CommandParameterFormatException("Invalid mention format.");
        }
    }

    public static void RegisterCustomParser(Type type, Func<ReadOnlyMemory<char>, object?> parser)
    {
        Parsers[type] = parser;
        _frozenParsers = Parsers.ToFrozenDictionary();
    }
}