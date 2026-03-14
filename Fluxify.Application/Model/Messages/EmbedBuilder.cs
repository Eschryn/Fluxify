using System.Drawing;
using Fluxify.Application.Model.Messages.Embeds;

namespace Fluxify.Application.Model.Messages;

public class EmbedBuilder 
{
    private Embed _embed = new Embed();
    private List<EmbedField>? _fields;
    public EmbedBuilder WithTitle(string title)
    {
        _embed.Title = title;
        return this;
    }

    public EmbedBuilder WithDescription(string description)
    {
        _embed.Description = description;
        return this;
    }

    public EmbedBuilder WithUrl(string url)
    {
        _embed.Url = url;
        return this;
    }

    public EmbedBuilder WithColor(Color color)
    {
        _embed.Color = color;
        return this;
    }

    public EmbedBuilder WithTimestamp(DateTimeOffset timestamp)
    {
        _embed.Timestamp = timestamp;
        return this;
    }
    
    public EmbedBuilder WithImage(string url, string? description = null)
    {
        _embed.Image = new EmbedMedia
        {
            Description = description,
            Url = url
        };
        
        return this;
    }

    public EmbedBuilder WithFooter(string text, string? iconUrl = null)
    {
        _embed.Footer = new EmbedFooter
        {
            IconUrl = iconUrl,
            Text = text
        };
        return this;
    }

    public EmbedBuilder WithAuthor(string name, string? iconUrl = null, string? url = null)
    {
        _embed.Author = new EmbedAuthor
        {
            IconUrl = iconUrl,
            Name = name,
            Url = url
        };
        
        return this;
    }

    public EmbedBuilder WithThumbnail(string url, string? description = null)
    {
        _embed.Thumbnail = new EmbedMedia
        {
            Url = url
        };
        
        return this;
    }

    public EmbedBuilder WithField(string name, string value, bool inline = false)
    {
        _fields ??= [];
        _fields.Add(new EmbedField
        {
            Name = name,
            Value = value,
            Inline = inline
        });
        return this;
    }

    public Embed Build()
    {
        _embed.Fields = _fields?.ToArray();
        return _embed;
    }
}