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
using Fluxify.Core.Types;
using Fluxify.Dto.Channels.Text.Messages.Embeds;

namespace Fluxify.Dto.Channels.Text.Messages.Search;

public record MessageSearchRequest(
    [property: Range(1, 25)] int? HitsPerPage,
    int? Page,
    Snowflake? MaxId,
    Snowflake? MinId,
    [property: StringLength(1024)] string? Content,
    [property: MaxLength(100)] string[]? Contents,
    [property: MaxLength(10)] string[]? ExactPhrases,
    [property: MaxLength(500)] Snowflake[]? ChannelId,
    [property: MaxLength(500)] Snowflake[]? ExcludeChannelId,
    MessageAuthorType[]? AuthorType,
    MessageAuthorType[]? ExcludeAuthorType,
    Snowflake[]? AuthorId,
    Snowflake[]? ExcludeAuthorId,
    Snowflake[]? Mentions,
    Snowflake[]? ExcludeMentions,
    bool? MentionEveryone,
    bool? Pinned,
    MessageContentType[]? Has,
    MessageContentType[]? ExcludeHas,
    MessageEmbedType[]? EmbedType,
    MessageEmbedType[]? ExcludeEmbedType,
    string[]? EmbedProvider,
    string[]? ExcludeEmbedProvider,
    string[]? LinkHostname,
    string[]? ExcludeLinkHostname,
    string[]? AttachmentFilename,
    string[]? ExcludeAttachmentFilename,
    string[]? AttachmentExtension,
    string[]? ExcludeAttachmentExtension,
    MessageSortField? SortField,
    MessageSortOrder? SortOrder,
    bool? IncludeNsfw,
    MessageSearchScope? Scope
);