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

using Fluxify.Commands.Exceptions;
using Fluxify.Core.Types;

namespace Fluxify.Commands.TextCommand;

public class CommandReader(CommandTokenizer tokenizer)
{
    public T GetNext<T>()
    {
        var next = GetNext(true);
        if (next is ReadOnlyMemory<char> memory)
        {
            return (T)ParseMemory(memory, typeof(T));
        }
        
        if (next is T value)
            return value;
        
        throw new CommandException($"Invalid arguments provided. Expected: {typeof(T).Name}, got {next.GetType().Name}.");
    }

    private object ParseMemory(ReadOnlyMemory<char> memory, Type type)
    {
        try
        {
            if (type == typeof(int))
                return int.Parse(memory.Span);
            if (type == typeof(uint))
                return uint.Parse(memory.Span);
            if (type == typeof(bool))
                return bool.Parse(memory.Span);
            if (type == typeof(sbyte))
                return sbyte.Parse(memory.Span);
            if (type == typeof(byte))
                return byte.Parse(memory.Span);
            if (type == typeof(short))
                return short.Parse(memory.Span);
            if (type == typeof(ushort))
                return ushort.Parse(memory.Span);
            if (type == typeof(long))
                return long.Parse(memory.Span);
            if (type == typeof(ulong))
                return ulong.Parse(memory.Span);
            if (type == typeof(char))
                return memory.Span[0];
            if (type == typeof(decimal))
                return decimal.Parse(memory.Span);
            if (type == typeof(float))
                return float.Parse(memory.Span);
            if (type == typeof(double))
                return double.Parse(memory.Span);
            if (type == typeof(nint))
                return nint.Parse(memory.Span);
            if (type == typeof(nuint))
                return nuint.Parse(memory.Span);
            if (type == typeof(nint))
                return nint.Parse(memory.Span);
            if (type == typeof(string))
                return memory.ToString();
            if (type == typeof(char[]))
                return memory.ToArray();
            if (type == typeof(ReadOnlyMemory<char>))
                return memory;
        }
        catch (FormatException e)
        {
            throw new CommandException($"Invalid argument provided. Malformed {type.Name}.");
        }
        
        throw new NotSupportedException();
    }

    public string GetNextString() => GetNext(true).ToString() ?? throw new CommandException("Invalid arguments provided.");
    
    public object GetNext(bool ignoreSpace = false)
    {
        var token = tokenizer.Next(out _);
        switch (token.Span)
        {
            case "<":
                return ParseMention();
            default: 
                return token;
        }
    }

    private Mentionable ParseMention()
    {
        try
        {
            var type = tokenizer.Next(out _).Span;
            var snowflakeStr = tokenizer.Next(out _);
            
            switch (type)
            {
                case "@":
                    switch (snowflakeStr.Span)
                    {
                        case "!":
                            snowflakeStr = tokenizer.Next(out _);
                            return new Mentionable.Member(ulong.Parse(snowflakeStr.Span));
                        case "&":
                            snowflakeStr = tokenizer.Next(out _);
                            return new Mentionable.Role(ulong.Parse(snowflakeStr.Span));
                        default:
                            return new Mentionable.Member(ulong.Parse(snowflakeStr.Span));
                    }
                case "#":
                    return new Mentionable.Channel(ulong.Parse(snowflakeStr.Span));
                case ":":
                    var name = tokenizer.Next(out _).Span;
                    if (tokenizer.Next(out _).Span is not ":")
                        throw new FormatException();
                    
                    var emojiId = tokenizer.Next(out _).Span;
                    return new Mentionable.Emoji(name.ToString(), ulong.Parse(emojiId));
                case "t":
                    if (snowflakeStr.Span is not ":")
                        throw new FormatException();
                    
                    var dateTimeStr = tokenizer.Next(out _).Span;
                    var dateTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(dateTimeStr));

                    if (tokenizer.Next(out _).Span is not ":")
                        return new Mentionable.DateTime(dateTime, "R");
                    
                    var format = tokenizer.Next(out _).Span;
                    return new Mentionable.DateTime(dateTime, format.ToString());
                default:
                    throw new FormatException();
            }
        }
        finally
        {
            if (tokenizer.Next(out _).Span is not ">") 
                throw new FormatException();
        }
    }
}

public abstract record Mentionable
{
    public record Role(Snowflake Id) : Mentionable
    {
        public override string ToString() => "<@&" + Id + ">";
    }

    public record Channel(Snowflake Id) : Mentionable
    {
        public override string ToString() => "<#" + Id + ">";
    }

    public record Member(Snowflake Id) : Mentionable
    {
        public override string ToString() => "<@" + Id + ">";
    }

    public record Emoji(string Name, Snowflake Id) : Mentionable
    {
        public override string ToString() => $"<:{Name}:{Id}>";
    }

    public record DateTime(DateTimeOffset Value, string Format) : Mentionable
    {
        public override string ToString() => $"<t:{Value}:{Format}>";
    }
}