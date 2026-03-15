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

using Fluxify.Core.Types;

namespace Fluxify.Dto.SavedMedia;

public record FavoriteMemeResponse(
    string? AltText,
    Snowflake? AttachmentId,
    string? ContentHash,
    string ContentType,
    double? Duration,
    string Filename,
    int? Height,
    int? Width,
    Snowflake Id,
    bool? IsGifv,
    string? KlipySlug,
    string Name,
    long Size,
    string[] Tags,
    string? TenorSlugId,
    string Url,
    Snowflake UserId
);