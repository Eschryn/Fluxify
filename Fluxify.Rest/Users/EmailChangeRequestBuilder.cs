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
    private const string ApplyUrl = "users/@me/email-change/apply";
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
        request: new EmailChangeVerifyOriginalRequest(code, ticket),
        DtoJsonContext.Default.EmailChangeVerifyOriginalRequest,
        DtoJsonContext.Default.EmailChangeVerifyOriginalResponse,
        bucket: RateLimitDefaults.UserEmailChangeVerifyOriginal,
        cancellationToken: cancellationToken
    );

    /// <summary>
    /// Requests to bind a new email to the ticket and verify it.
    /// </summary>
    /// <param name="request">The request param object for this operation.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    /// <returns>Info about the code that was sent to the new email.</returns>
    /// <exception cref="RestApiException">This exception is thrown for tickets that require original email verification and the original email hasn't been verified yet (ORIGINAL_EMAIL_MUST_BE_VERIFIED_FIRST) or if the proof doesn't match the token it will return the error INVALID_PROOF_TOKEN.</exception>
    /// <exception cref="RateLimitException">This exception is thrown when a rate limit has been hit without the <see cref="FluxerRateLimitingHandler"/>.</exception>
    /// <seealso href="https://docs.fluxer.app/http-api/users/email-and-password/#request-new-email"/>
    /// <remarks>
    /// After getting a ticket with start, this is your starting point for the account claiming process.
    /// </remarks>
    public Task<EmailChangeRequestNewResponse> RequestNewAsync(
        EmailChangeRequestNewRequest request,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<EmailChangeRequestNewRequest, EmailChangeRequestNewResponse>(
        HttpMethod.Post,
        RequestNewUrl,
        request,
        DtoJsonContext.Default.EmailChangeRequestNewRequest,
        DtoJsonContext.Default.EmailChangeRequestNewResponse,
        bucket: RateLimitDefaults.UserEmailChangeRequestNew,
        cancellationToken: cancellationToken
    );

    /// <summary>
    /// Requests to send a new verification code to the users new email.
    /// </summary>
    /// <param name="ticket">The ticket of the email change process.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    /// <exception cref="RestApiException">This exception is thrown for every ticket that doesn't request a new email with NO_NEW_EMAIL_REQUESTED.</exception>
    /// <exception cref="RateLimitException">This exception is thrown when a rate limit has been hit without the <see cref="FluxerRateLimitingHandler"/>.</exception>
    /// <seealso href="https://docs.fluxer.app/http-api/users/email-and-password/#resend-original-email-code"/>
    public Task ResendNewAsync(
        string ticket,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync(
        HttpMethod.Post,
        ResendNewUrl,
        request: new EmailChangeTicketRequest(ticket),
        DtoJsonContext.Default.EmailChangeTicketRequest,
        bucket: RateLimitDefaults.UserEmailChangeResendNew,
        cancellationToken: cancellationToken
    );

    /// <summary>
    /// Verifies the new email of the user.
    /// </summary>
    /// <param name="ticket">The ticket for which the new email should be verified.</param>
    /// <param name="originalProof">The proof for the original email/proof returned from start.</param>
    /// <param name="code">The code that was sent to the new email.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    /// <returns>A response object that contains the email token for completing the change.</returns>
    /// <exception cref="RestApiException">This exception is thrown for every ticket where the proof doesn't belong to the token (INVALID_PROOF_TOKEN), verification was not issued yet (VERIFICATION_CODE_NOT_ISSUED), the code expired (VERIFICATION_CODE_EXPIRED), the code is invalid (INVALID_VERIFICATION_CODE) or the email was already verified (TICKET_ALREADY_COMPLETED).</exception>
    /// <exception cref="RateLimitException">This exception is thrown when a rate limit has been hit without the <see cref="FluxerRateLimitingHandler"/>.</exception>
    /// <seealso href="https://docs.fluxer.app/http-api/users/email-and-password/#verify-new-email"/>
    public Task<EmailTokenResponse> VerifyNewAsync(
        string ticket,
        string originalProof,
        string code,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<EmailChangeVerifyNewRequest, EmailTokenResponse>(
        HttpMethod.Post,
        VerifyNewUrl,
        request: new EmailChangeVerifyNewRequest(originalProof, code, ticket),
        DtoJsonContext.Default.EmailChangeVerifyNewRequest,
        DtoJsonContext.Default.EmailTokenResponse,
        bucket: RateLimitDefaults.UserEmailChangeVerifyNew,
        cancellationToken: cancellationToken
    );

    /// <summary>
    /// Applies the email change.
    /// </summary>
    /// <param name="request">Parameter object for the request.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    /// <returns>The updated private user object.</returns>
    /// <exception cref="RestApiException">This exception is thrown when token belongs to another account (INVALID_EMAIL_TOKEN), the token is past its lifetime (EMAIL_TOKEN_EXPIRED), when another account claimed the email in the meanwhile (EMAIL_ALREADY_IN_USE) or token has been used again (INVALID_EMAIL_TOKEN).</exception>
    /// <exception cref="RateLimitException">This exception is thrown when a rate limit has been hit without the <see cref="FluxerRateLimitingHandler"/>.</exception>
    /// <exception cref="NotPermittedException">This exception is thrown when sudo mode parameter weren't properly provided (SUDO_MODE_REQUIRED).</exception>
    /// <seealso href="https://docs.fluxer.app/http-api/users/email-and-password/#apply-email-change"/>
    public Task<UserPrivateReponse> ApplyChangeAsync(
        EmailChangeApplyRequest request,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<, UserPrivateReponse>(
        HttpMethod.Post,
        ApplyUrl,
        request: request,
        DtoJsonContext.Default.EmailChangeApplyRequest,
        DtoJsonContext.Default.UserPrivateReponse,
        bucket: RateLimitDefaults.UserEmailChangeApply,
        cancellationToken: cancellationToken
    );

    /// <summary>
    /// Requests to start the email recovery flow.
    /// </summary>
    /// <param name="newEmail">The new email that should be linked to the account.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    /// <returns>Ticket and info about the code that was sent to the new email.</returns>
    /// <exception cref="RestApiException">When the user account still holds an email the process will fail with ORIGINAL_EMAIL_MUST_BE_VERIFIED_FIRST.</exception>
    /// <exception cref="RateLimitException">This exception is thrown when a rate limit has been hit without the <see cref="FluxerRateLimitingHandler"/>.</exception>
    /// <seealso href="https://docs.fluxer.app/http-api/users/email-and-password/#request-replacement-email-for-bounced-address"/>
    public Task<EmailChangeRequestNewResponse> BouncedRequestNewAsync(
        string newEmail,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<EmailChangeBouncedRequestNewRequest, EmailChangeRequestNewResponse>(
        HttpMethod.Post,
        BouncedRequestNewUrl,
        request: new EmailChangeBouncedRequestNewRequest(newEmail),
        DtoJsonContext.Default.EmailChangeBouncedRequestNewRequest,
        DtoJsonContext.Default.EmailChangeRequestNewResponse,
        bucket: RateLimitDefaults.UserEmailChangeBouncedRequestNew,
        cancellationToken: cancellationToken
    );

    /// <summary>
    /// Requests to send a new verification code to the users replacement email.
    /// </summary>
    /// <param name="ticket">The ticket of the bounced email change process.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    /// <exception cref="RestApiException">This exception is thrown when the has no replacement email with NO_NEW_EMAIL_REQUESTED.</exception>
    /// <exception cref="NotPermittedException">This exception is thrown when the user was not marked as bounced with ACCESS_DENIED.</exception>
    /// <exception cref="RateLimitException">This exception is thrown when a rate limit has been hit without the <see cref="FluxerRateLimitingHandler"/>.</exception>
    /// <seealso href="https://docs.fluxer.app/http-api/users/email-and-password/#request-replacement-email-for-bounced-address"/>
    public Task BouncedResendNewAsync(
        string ticket,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync(
        HttpMethod.Post,
        BouncedResendNewUrl,
        request: new EmailChangeTicketRequest(ticket),
        DtoJsonContext.Default.EmailChangeTicketRequest,
        bucket: RateLimitDefaults.UserEmailChangeBouncedResendNew,
        cancellationToken: cancellationToken
    );

    /// <summary>
    /// Verifies the replacement email for the bounced user account.
    /// </summary>
    /// <param name="code">The code that was sent to the users replacement email.</param>
    /// <param name="ticket">The ticket of the bounced email change process.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    /// <returns>The updated private user object.</returns>
    /// <exception cref="NotPermittedException">This exception is thrown when the user was not marked as bounced with ACCESS_DENIED.</exception>
    /// <exception cref="RateLimitException">This exception is thrown when a rate limit has been hit without the <see cref="FluxerRateLimitingHandler"/>.</exception>
    /// <seealso href="https://docs.fluxer.app/http-api/users/email-and-password/#verify-replacement-email-for-bounced-address"/>
    public Task<UserPrivateReponse> BouncedVerifyNewAsync(
        string ticket,
        string code,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<EmailChangeBouncedRequestVerifyNewRequest, UserPrivateReponse>(
        HttpMethod.Post,
        BouncedVerifyNewUrl,
        request: new EmailChangeBouncedRequestVerifyNewRequest(code, ticket),
        DtoJsonContext.Default.EmailChangeBouncedRequestVerifyNewRequest,
        DtoJsonContext.Default.UserPrivateReponse,
        bucket: RateLimitDefaults.UserEmailChangeBouncedVerifyNew,
        cancellationToken: cancellationToken
    );
}