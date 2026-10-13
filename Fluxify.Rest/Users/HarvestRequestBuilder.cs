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
using Fluxify.Dto.Users.DataHarvest;

namespace Fluxify.Rest.Users;

/// <summary>
/// Provides data harvest related requests. 
/// </summary>
/// <param name="client">The http client which should be used to send requests.</param>
public class HarvestRequestBuilder(HttpClient client)
{
    private static readonly IFormatProvider FormatProvider = CultureInfo.InvariantCulture;
    private const string HarvestUrl = "users/@me/harvest";
    private const string HarvestFilteredUrl = "users/@me/harvest/filtered";
    private const string LatestUrl = "users/@me/harvest/latest";
    private static readonly CompositeFormat HarvestIdUrl = CompositeFormat.Parse("users/@me/harvest/{0}");
    private static readonly CompositeFormat HarvestIdDownloadUrl = CompositeFormat.Parse("users/@me/harvest/{0}/download");
    
    /// <summary>
    /// Requests a data harvest of the user's data.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    /// <returns>Response object containing information about the requested data harvest operation.</returns>
    /// <exception cref="NotFoundException">This exception is thrown when the authenticated account no longer resolves with UNKNONW_USER.</exception>
    /// <exception cref="RateLimitException">This exception is thrown when a rate limit has been hit without the <see cref="FluxerRateLimitingHandler"/>.</exception>
    /// <seealso href="https://docs.fluxer.app/http-api/users/data-harvest/#request-data-harvest"/>
    public Task<HarvestCreationResponseSchema> RequestAsync(
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<HarvestCreationResponseSchema>(
        HttpMethod.Post,
        HarvestUrl,
        DtoJsonContext.Default.HarvestCreationResponseSchema,
        bucket: RateLimitDefaults.UserDataHarvest,
        cancellationToken: cancellationToken
    );

    /// <summary>
    /// Requests a data harvest of the user's data.
    /// </summary>
    /// <param name="filter">Filtering options for the data harvest.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    /// <returns>Response object containing information about the requested data harvest operation.</returns>
    /// <exception cref="RestApiException">This exception is thrown when the request object fails validation.</exception>
    /// <exception cref="NotFoundException">This exception is thrown when the authenticated account no longer resolves with UNKNOWN_USER.</exception>
    /// <exception cref="RateLimitException">This exception is thrown when a rate limit has been hit without the <see cref="FluxerRateLimitingHandler"/>.</exception>
    /// <seealso href="https://docs.fluxer.app/http-api/users/data-harvest/#request-filtered-data-harvest"/>
    public Task<HarvestCreationResponseSchema> RequestFilteredAsync(
        HarvestMessageFilter filter,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<HarvestMessageFilter, HarvestCreationResponseSchema>(
        HttpMethod.Post,
        HarvestFilteredUrl,
        request: filter,
        requestJsonInfo: DtoJsonContext.Default.HarvestMessageFilter,
        resultJsonInfo: DtoJsonContext.Default.HarvestCreationResponseSchema,
        bucket: RateLimitDefaults.UserDataHarvest,
        cancellationToken: cancellationToken
    );
    
    /// <summary>
    /// Gets the status of the most recently requested harvest.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    /// <returns>Response object containing information about the status of the harvest.</returns>
    /// <exception cref="RateLimitException">This exception is thrown when a rate limit has been hit without the <see cref="FluxerRateLimitingHandler"/>.</exception>
    /// <seealso href="https://docs.fluxer.app/http-api/users/data-harvest/#get-latest-data-harvest"/>
    public Task<HarvestStatusResponseSchema> GetLatestAsync(
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<HarvestStatusResponseSchema>(
        HttpMethod.Get,
        LatestUrl,
        DtoJsonContext.Default.HarvestStatusResponseSchema,
        bucket: RateLimitDefaults.UserHarvestLatest,
        cancellationToken: cancellationToken
    );
    
    /// <summary>
    /// Gets the status of the specified harvest.
    /// </summary>
    /// <param name="harvestId">The ID of the harvest that should be queried for status.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    /// <returns>Response object containing information about the status of the harvest.</returns>
    /// <exception cref="RateLimitException">This exception is thrown when a rate limit has been hit without the <see cref="FluxerRateLimitingHandler"/>.</exception>
    /// <exception cref="NotFoundException">This exception is thrown when the harvest id is invalid or does not belong to this account.</exception>
    /// <seealso href="https://docs.fluxer.app/http-api/users/data-harvest/#get-data-harvest-status"/>
    public Task<HarvestStatusResponseSchema> GetAsync(
        Snowflake harvestId,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<HarvestStatusResponseSchema>(
        HttpMethod.Get,
        string.Format(FormatProvider, HarvestIdUrl, harvestId),
        DtoJsonContext.Default.HarvestStatusResponseSchema,
        bucket: RateLimitDefaults.UserHarvestStatus,
        cancellationToken: cancellationToken
    );
    
    /// <summary>
    /// Gets the download url for the requested harvest.
    /// </summary>
    /// <param name="harvestId">The ID of the harvest for which the download url should be retrieved.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    /// <returns>Response object containing the download url.</returns>
    /// <exception cref="RateLimitException">This exception is thrown when a rate limit has been hit without the <see cref="FluxerRateLimitingHandler"/>.</exception>
    /// <exception cref="NotFoundException">This exception is thrown when the harvest id is invalid or does not belong to this account.</exception>
    /// <exception cref="RestApiException">This exception is thrown when the harvest is not ready, failed or expired.</exception>
    /// <seealso href="https://docs.fluxer.app/http-api/users/data-harvest/#get-data-harvest-download-url"/>
    public Task<HarvestDownloadUrlResponse> GetDownloadAsync(
        Snowflake harvestId,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<HarvestDownloadUrlResponse>(
        HttpMethod.Get,
        string.Format(FormatProvider, HarvestIdDownloadUrl, harvestId),
        DtoJsonContext.Default.HarvestDownloadUrlResponse,
        bucket: RateLimitDefaults.UserHarvestDownload,
        cancellationToken: cancellationToken
    );
}