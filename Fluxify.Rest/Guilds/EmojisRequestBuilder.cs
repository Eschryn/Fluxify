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

using System.Globalization;
using System.Text;
using Fluxify.Core.Types;
using Fluxify.Dto.Guilds.Emoji;

namespace Fluxify.Rest.Guilds;

/// <summary>
/// Exposes emoji related requests.
/// </summary>
/// <param name="client">The http client that should be used to send requests.</param>
/// <param name="guildId">The id of the guild that the emojis are scoped to.</param>
public class EmojisRequestBuilder(HttpClient client, Snowflake guildId)
{
    private static readonly CultureInfo FormatProvider = CultureInfo.InvariantCulture;
    private static readonly CompositeFormat EmojisUrl = CompositeFormat.Parse("guilds/{0}/emojis");
    private static readonly CompositeFormat EmojiUrl = CompositeFormat.Parse("guilds/{0}/emojis/{1}");
    private static readonly CompositeFormat BulkEmojisUrl = CompositeFormat.Parse("guilds/{0}/emojis/bulk");
    private static readonly CompositeFormat CloneEmojiUrl = CompositeFormat.Parse("guilds/{0}/emojis/clone");

    /// <summary>
    /// Creates an emoji in a guild.
    /// </summary>
    /// <param name="request">The request body to create the emoji.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    /// <param name="reason">The reason that should be displayed in the audit log.</param>
    /// <returns>The response object of the created emoji, if successful.</returns>
    /// <exception cref="NotFoundException">This exception is thrown when the guild does not exist.</exception>
    /// <exception cref="NotPermittedException">This exception is thrown when the name or image hash is banned or the caller lacks permissions.</exception>
    /// <exception cref="RestApiException">This exception is thrown when the guild is at the emoji slot limit, the name is undecodable or in an unsupported format.</exception>
    /// <exception cref="RateLimitException">This exception is thrown when a rate limit has been hit without the <see cref="FluxerRateLimitingHandler"/>.</exception>
    /// <remarks>
    /// This operation requires the permission <see cref="Permissions.CreateExpressions"/>.
    /// </remarks>
    /// <seealso href="https://docs.fluxer.app/http-api/guild-emojis/#create-guild-emoji"/>
    public Task<GuildEmojiResponse> CreateAsync(
        GuildEmojiCreateRequest request,
        string? reason = null,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<GuildEmojiCreateRequest, GuildEmojiResponse>(
        HttpMethod.Post,
        string.Format(FormatProvider, EmojisUrl, guildId),
        request,
        DtoJsonContext.Default.GuildEmojiCreateRequest,
        DtoJsonContext.Default.GuildEmojiResponse,
        bucket: RateLimitDefaults.GuildEmojiCreate,
        reason: reason,
        cancellationToken: cancellationToken
    );

    /// <summary>
    /// Creates multiple emojis in a guild.
    /// </summary>
    /// <param name="request">The request body that contains all emojis that should be created.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    /// <param name="reason">The reason that should be displayed in the audit log.</param>
    /// <returns>Response object that contains all response objects of the created emojis, if successful.</returns>
    /// <exception cref="NotFoundException">This exception is thrown when the guild does not exist.</exception>
    /// <exception cref="NotPermittedException">This exception is thrown when the name or image hash is banned or the caller lacks permissions.</exception>
    /// <exception cref="RestApiException">This exception is thrown when the guild is at the emoji slot limit, the name is undecodable or in an unsupported format.</exception>
    /// <exception cref="RateLimitException">This exception is thrown when a rate limit has been hit without the <see cref="FluxerRateLimitingHandler"/>.</exception>
    /// <remarks>
    /// This operation requires the permission <see cref="Permissions.CreateExpressions"/>.
    /// </remarks>
    /// <seealso href="https://docs.fluxer.app/http-api/guild-emojis/#bulk-create-guild-emojis"/>
    public Task<GuildEmojiBulkCreateResponse> BulkCreateAsync(
        GuildEmojiBulkCreateRequest request,
        string? reason = null,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<GuildEmojiBulkCreateRequest, GuildEmojiBulkCreateResponse>(
        HttpMethod.Post,
        string.Format(FormatProvider, BulkEmojisUrl, guildId),
        request,
        DtoJsonContext.Default.GuildEmojiBulkCreateRequest,
        DtoJsonContext.Default.GuildEmojiBulkCreateResponse,
        bucket: RateLimitDefaults.GuildEmojiBulkCreate,
        reason: reason,
        cancellationToken: cancellationToken
    );

    /// <summary>
    /// Deletes an emoji in a guild.
    /// </summary>
    /// <param name="emojiId">The id of the emoji that should be deleted.</param>
    /// <param name="purge">Whether the image of the emoji should be deleted permanently.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    /// <param name="reason">The reason that should be displayed in the audit log.</param>
    /// <exception cref="NotFoundException">This exception is thrown when the guild or the emoji does not exist.</exception>
    /// <exception cref="NotPermittedException">This exception is thrown when the caller lacks permissions by being neither creator and with <see cref="Permissions.CreateExpressions"/> permission or have the <see cref="Permissions.ManageExpressions"/> permission.</exception>
    /// <exception cref="RateLimitException">This exception is thrown when a rate limit has been hit without the <see cref="FluxerRateLimitingHandler"/>.</exception>
    /// <remarks>
    /// This operation requires the permission <see cref="Permissions.ManageExpressions"/> or <see cref="Permissions.CreateExpressions"/>.
    /// </remarks>
    /// <seealso href="https://docs.fluxer.app/http-api/guild-emojis/#delete-guild-emoji"/>
    public Task DeleteEmojiAsync(
        Snowflake emojiId,
        bool? purge = null,
        string? reason = null,
        CancellationToken cancellationToken = default
    ) => client.RequestAsync(
        HttpMethod.Delete,
        string.Format(FormatProvider, EmojiUrl, guildId, emojiId) + new QueryBuilder()
            .AddQuery("purge", purge?.ToString().ToLowerInvariant()),
        bucket: RateLimitDefaults.GuildEmojiDelete,
        reason: reason,
        cancellationToken: cancellationToken
    );

    /// <summary>
    /// Updates an emoji in a guild.
    /// </summary>
    /// <param name="emojiId">The id of the emoji to be updated.</param>
    /// <param name="request">The request body to update the emoji.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    /// <param name="reason">The reason that should be displayed in the audit log.</param>
    /// <exception cref="RestApiException">This exception is thrown when the name is invalid.</exception>
    /// <exception cref="NotFoundException">This exception is thrown when the guild or the emoji does not exist.</exception>
    /// <exception cref="NotPermittedException">This exception is thrown when the caller lacks permissions by being neither creator and with <see cref="Permissions.CreateExpressions"/> permission or have the <see cref="Permissions.ManageExpressions"/> permission.</exception>
    /// <exception cref="RateLimitException">This exception is thrown when a rate limit has been hit without the <see cref="FluxerRateLimitingHandler"/>.</exception>
    /// <remarks>
    /// This operation requires the permission <see cref="Permissions.ManageExpressions"/> or <see cref="Permissions.CreateExpressions"/>.
    /// </remarks>
    /// <seealso href="https://docs.fluxer.app/http-api/guild-emojis/#modify-guild-emoji"/>
    public Task UpdateEmojiAsync(
        Snowflake emojiId,
        GuildEmojiUpdateRequest request,
        string? reason = null,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync(
        HttpMethod.Patch,
        string.Format(FormatProvider, EmojiUrl, guildId, emojiId),
        request,
        DtoJsonContext.Default.GuildEmojiUpdateRequest,
        bucket: RateLimitDefaults.GuildEmojiUpdate,
        reason: reason,
        cancellationToken: cancellationToken
    );

    /// <summary>
    /// Clones an emoji from another guild.
    /// </summary>
    /// <param name="sourceEmojiId">The id of the emoji that should be cloned.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    /// <param name="reason">The reason that should be displayed in the audit log.</param>
    /// <exception cref="RestApiException">This exception is thrown when the target guild is at the emoji slot limit.</exception>
    /// <exception cref="NotFoundException">This exception is thrown when the guild or the emoji does not exist.</exception>
    /// <exception cref="NotPermittedException">This exception is thrown when the caller is not a member of the target guild or lacks <see cref="Permissions.CreateExpressions"/> permission there or the source guild does not permit cloning.</exception>
    /// <exception cref="RateLimitException">This exception is thrown when a rate limit has been hit without the <see cref="FluxerRateLimitingHandler"/>.</exception>
    /// <remarks>
    /// This operation requires the permission <see cref="Permissions.CreateExpressions"/>.
    /// </remarks>
    /// <seealso href="https://docs.fluxer.app/http-api/guild-emojis/#clone-guild-emoji"/>
    public Task<GuildEmojiResponse> CloneEmojiAsync(
        Snowflake sourceEmojiId,
        string? reason = null,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<GuildEmojiCloneRequest, GuildEmojiResponse>(
        HttpMethod.Post,
        string.Format(FormatProvider, CloneEmojiUrl, guildId),
        request: new GuildEmojiCloneRequest(sourceEmojiId),
        requestJsonInfo: DtoJsonContext.Default.GuildEmojiCloneRequest,
        resultJsonInfo: DtoJsonContext.Default.GuildEmojiResponse,
        bucket: RateLimitDefaults.GuildEmojiClone,
        reason: reason,
        cancellationToken: cancellationToken
    );
}