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

using Fluxify.Dto.Users;
using Fluxify.Dto.Users.Settings.EmailChange;

namespace Fluxify.Rest.Users;

/// <summary>
/// Provides email change requests
/// </summary>
/// <param name="client">The http client which should be used to send requests.</param>
public class EmailChangeRequestBuilder(HttpClient client)
{
    private const string BouncedRequestNewUrl = "users/@me/email-change/bounced/request-new";
    private const string BouncedResendNewUrl = "users/@me/email-change/bounced/resend-new";
    private const string BouncedVerifyNewUrl = "users/@me/email-change/bounced/verify-new";
    private const string RequestNewUrl = "users/@me/email-change/request-new";
    private const string ResendNewUrl = "users/@me/email-change/resend-new";
    private const string ResendOriginalUrl = "users/@me/email-change/resend-original";
    private const string StartUrl = "users/@me/email-change/start";
    private const string VerifyNewUrl = "users/@me/email-change/verify-new";
    private const string VerifyOriginalUrl = "users/@me/email-change/verify-original";
    
    /// <summary>
    /// Requests to start the email change process.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    /// <returns>Operation response containing the ticket for the email change process.</returns>
    /// <exception cref="RestApiException">This exception is thrown for every user that is not unclaimed or has no email.</exception>
    /// <exception cref="RateLimitException">This exception is thrown when a rate limit has been hit without the <see cref="FluxerRateLimitingHandler"/>.</exception>
    /// <seealso href="https://docs.fluxer.app/http-api/users/email-and-password/#start-email-change"/>
    public Task<EmailChangeStartResponse> StartAsync(
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<EmailChangeStartResponse>(
        HttpMethod.Post,
        StartUrl,
        DtoJsonContext.Default.EmailChangeStartResponse,
        bucket: RateLimitDefaults.UserEmailChangeStart,
        cancellationToken: cancellationToken
    );
    
    /// <summary>
    /// Requests to send a new verification code to the users original email.
    /// </summary>
    /// <param name="ticket">The ticket of the email change process.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    /// <exception cref="RestApiException">This exception is thrown for every ticket that doesn't change the original email and users that have their original email already verified.</exception>
    /// <exception cref="RateLimitException">This exception is thrown when a rate limit has been hit without the <see cref="FluxerRateLimitingHandler"/>.</exception>
    /// <seealso href="https://docs.fluxer.app/http-api/users/email-and-password/#resend-original-email-code"/>
    public Task ResendOriginalAsync(
        string ticket,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync(
        HttpMethod.Post,
        ResendOriginalUrl,
        request: new EmailChangeTicketRequest(ticket),
        DtoJsonContext.Default.EmailChangeTicketRequest,
        bucket: RateLimitDefaults.UserEmailChangeResendOriginal,
        cancellationToken: cancellationToken
    );

    /// <summary>
    /// Verifies the original email of the user.
    /// </summary>
    /// <param name="ticket">The ticket of the email change process.</param>
    /// <param name="code">The code that was sent to the original email.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    /// <returns>Verification proof.</returns>
    /// <exception cref="RestApiException">This exception is thrown for every ticket that doesn't change the original email and users that have their original email already verified.</exception>
    /// <exception cref="RateLimitException">This exception is thrown when a rate limit has been hit without the <see cref="FluxerRateLimitingHandler"/>.</exception>
    /// <seealso href="https://docs.fluxer.app/http-api/users/email-and-password/#verify-original-email"/>
    public Task<EmailChangeVerifyOriginalResponse> VerifyOriginalAsync(
        string ticket,
        string code,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<EmailChangeVerifyOriginalRequest, EmailChangeVerifyOriginalResponse>(
        HttpMethod.Post,
        VerifyOriginalUrl,
        request:  new EmailChangeVerifyOriginalRequest(code, ticket),
        DtoJsonContext.Default.EmailChangeVerifyOriginalRequest,
        DtoJsonContext.Default.EmailChangeVerifyOriginalResponse,
        bucket: RateLimitDefaults.UserEmailChangeVerifyOriginal,
        cancellationToken: cancellationToken
    );
    
    public Task<EmailChangeRequestNewResponse> RequestNewAsync(
        EmailChangeRequestNewRequest request,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<EmailChangeRequestNewRequest, EmailChangeRequestNewResponse>(
        HttpMethod.Post,
        RequestNewUrl,
        request,
        DtoJsonContext.Default.EmailChangeRequestNewRequest,
        DtoJsonContext.Default.EmailChangeRequestNewResponse,
        cancellationToken: cancellationToken
    );
    
    public Task ResendNewAsync(
        EmailChangeTicketRequest request,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync(
        HttpMethod.Post,
        ResendNewUrl,
        request,
        DtoJsonContext.Default.EmailChangeTicketRequest,
        cancellationToken: cancellationToken
    );

    public Task<EmailTokenResponse> VerifyNewAsync(
        EmailChangeVerifyNewRequest request,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<EmailChangeVerifyNewRequest, EmailTokenResponse>(
        HttpMethod.Post,
        VerifyNewUrl,
        request,
        DtoJsonContext.Default.EmailChangeVerifyNewRequest,
        DtoJsonContext.Default.EmailTokenResponse,
        cancellationToken: cancellationToken
    );
    
    public Task<EmailChangeRequestNewResponse> BouncedRequestNewAsync(
        EmailChangeBouncedRequestNewRequest request,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<EmailChangeBouncedRequestNewRequest, EmailChangeRequestNewResponse>(
        HttpMethod.Post,
        BouncedRequestNewUrl,
        request,
        DtoJsonContext.Default.EmailChangeBouncedRequestNewRequest,
        DtoJsonContext.Default.EmailChangeRequestNewResponse,
        cancellationToken: cancellationToken
    );
    
    public Task BouncedResendNewAsync(
        EmailChangeTicketRequest request,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync(
        HttpMethod.Post,
        BouncedResendNewUrl,
        request,
        DtoJsonContext.Default.EmailChangeTicketRequest,
        cancellationToken: cancellationToken
    );
    
    public Task<UserPrivateReponse> BouncedVerifyNewAsync(
        EmailChangeBouncedRequestVerifyNewRequest request,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<EmailChangeBouncedRequestVerifyNewRequest, UserPrivateReponse>(
        HttpMethod.Post,
        BouncedVerifyNewUrl,
        request,
        DtoJsonContext.Default.EmailChangeBouncedRequestVerifyNewRequest,
        DtoJsonContext.Default.UserPrivateReponse,
        cancellationToken: cancellationToken
    );
}