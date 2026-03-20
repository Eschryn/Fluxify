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

namespace Fluxify.Application.Model.Messages.Embeds;

public sealed class Embed
{
    public string? Url { get; set; }
    public string? Title { get; set; }
    public Color? Color { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public string? Description { get; set; }
    public EmbedAuthor? Author { get; set; }
    public EmbedMedia? Image { get; set; }
    public EmbedMedia? Thumbnail { get; set; }
    public EmbedFooter? Footer { get; set; }
    public EmbedField[]? Fields { get; set; }
    public Embed[]? Children { get; internal init; }
    public bool? Nsfw { get; internal set; }
    public EmbedMedia? Video { get; internal set; }
    public EmbedMedia? Audio { get; internal set; }
    public EmbedType Type { get; internal set; } = EmbedType.Rich;
    public EmbedAuthor? Provider { get; internal set; }
}