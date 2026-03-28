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

namespace Fluxify.Dto.Channels.Text.Messages.Embeds.Request;

/// <summary>
/// The author of the embed
/// </summary>
/// <param name="IconUrl">The author's icon URL.</param>
/// <param name="Name">The author's name.</param>
/// <param name="Url">The author's URL.</param>
public sealed record EmbedAuthorRequest(
    [property: StringLength(2048)]
    string? IconUrl,
    [property: StringLength(256)]
    string Name,
    [property: StringLength(2048)]
    string? Url
);