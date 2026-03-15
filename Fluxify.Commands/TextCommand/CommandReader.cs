using System.Buffers;
using Fluxify.Commands.Exceptions;
using Fluxify.Core.Types;

namespace Fluxify.Commands.TextCommand;

public class CommandReader(CommandTokenizer tokenizer)
{
    public T GetNext<T>() where T : Mentionable
    {
        var next = GetNext(true);
        if (next is T value)
            return value;
        
        throw new CommandException($"Invalid arguments provided. Expected: {typeof(T).Name}, got {next.GetType().Name}.");
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
                return token.ToString();
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