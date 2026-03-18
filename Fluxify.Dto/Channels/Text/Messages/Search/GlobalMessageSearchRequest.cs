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
using Fluxify.Dto.Channels.Text.Messages.Embeds;

namespace Fluxify.Dto.Channels.Text.Messages.Search;

public sealed record GlobalMessageSearchRequest(
    int? HitsPerPage,
    int? Page,
    Snowflake? MaxId,
    Snowflake? MinId,
    string? Content,
    string[]? Contents,
    string[]? ExactPhrases,
    Snowflake[]? ChannelId,
    Snowflake[]? ExcludeChannelId,
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
    MessageSearchScope? Scope,
    Snowflake? ContextChannelId,
    Snowflake? ContextGuildId,
    Snowflake[]? ChannelIds,
    bool? Indexing
) : MessageSearchRequest(
    HitsPerPage,
    Page,
    MaxId,
    MinId,
    Content,
    Contents,
    ExactPhrases,
    ChannelId,
    ExcludeChannelId,
    AuthorType,
    ExcludeAuthorType,
    AuthorId,
    ExcludeAuthorId,
    Mentions,
    ExcludeMentions,
    MentionEveryone,
    Pinned,
    Has,
    ExcludeHas,
    EmbedType,
    ExcludeEmbedType,
    EmbedProvider,
    ExcludeEmbedProvider,
    LinkHostname,
    ExcludeLinkHostname,
    AttachmentFilename,
    ExcludeAttachmentFilename,
    AttachmentExtension,
    ExcludeAttachmentExtension,
    SortField,
    SortOrder,
    IncludeNsfw,
    Scope,
    Indexing
);