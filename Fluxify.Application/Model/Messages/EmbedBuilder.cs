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