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