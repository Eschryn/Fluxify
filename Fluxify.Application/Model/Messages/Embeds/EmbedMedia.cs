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

using System.ComponentModel.DataAnnotations;

namespace Fluxify.Application.Model.Messages.Embeds;

public sealed class EmbedMedia
{
    public string? ContentHash { get; internal set; }
    public string? ContentType { get; internal set; }
    public string? Description { get; set; }
    public int? Duration { get; internal set; }
    public EmbedMediaFlags Flags { get; internal set; }
    public int? Height { get; internal set; }
    public int? Width { get; internal set; }
    public string? Placeholder { get; internal set; }
    public required string Url { get; init; }
    public string? ProxyUrl { get; internal set; }
}