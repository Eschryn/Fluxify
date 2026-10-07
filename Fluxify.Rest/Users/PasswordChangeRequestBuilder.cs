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

using Fluxify.Dto.Users.Settings.PasswordChange;

namespace Fluxify.Rest.Users;

public class PasswordChangeRequestBuilder(HttpClient client)
{
    private const string CompleteUrl = "users/@me/password-change/complete";
    private const string ResendUrl = "users/@me/password-change/resend";
    private const string StartUrl = "users/@me/password-change/start";
    private const string VerifyUrl = "users/@me/password-change/verify";

    /// <summary>
    /// Requests to start a password change.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    /// <returns>Response object that contains the ticket of the process and information about the sent code.</returns>
    /// <exception cref="RestApiException">This exception is thrown for every user that has no email with MUST_HAVE_EMAIL_TO_CHANGE_IT.</exception>
    /// <exception cref="RateLimitException">This exception is thrown when a rate limit has been hit without the <see cref="FluxerRateLimitingHandler"/>.</exception>
    /// <seealso href="https://docs.fluxer.app/http-api/users/email-and-password/#start-password-change"/>
    public Task<PasswordChangeStartResponse> StartAsync(
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<PasswordChangeStartResponse>(
        HttpMethod.Post,
        StartUrl,
        DtoJsonContext.Default.PasswordChangeStartResponse,
        bucket: RateLimitDefaults.UserPasswordChangeStart,
        cancellationToken: cancellationToken
    );

    /// <summary>
    /// Requests to send a new code to the user's email address.
    /// </summary>
    /// <param name="ticket">The identifier of the password change process.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    /// <exception cref="RestApiException">This exception is thrown when the user requests to resend the password change code before ResendAvailableAt.</exception>
    /// <exception cref="RateLimitException">This exception is thrown when a rate limit has been hit without the <see cref="FluxerRateLimitingHandler"/>.</exception>
    /// <seealso href="https://docs.fluxer.app/http-api/users/email-and-password/#resend-password-change-code"/>
    public Task ResendAsync(
        string ticket,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync(
        HttpMethod.Post,
        ResendUrl,
        request: new PasswordChangeTicketRequest(ticket),
        DtoJsonContext.Default.PasswordChangeTicketRequest,
        bucket: RateLimitDefaults.UserPasswordChangeResend,
        cancellationToken: cancellationToken
    );

    /// <summary>
    /// Requests to complete verification of the password change process using the specified code.
    /// </summary>
    /// <param name="ticket">The identifier of the password change process.</param>
    /// <param name="code">The code that was sent to the user's email address.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    /// <returns>The response object containing the verification proof.</returns>
    /// <exception cref="RestApiException">This exception is thrown when the no verification code is issued with VERIFICATION_CODE_NOT_ISSUED, when the code expired with VERIFICATION_CODE_EXPIRED and when the code was invalid with INVALID_VERIFICATION_CODE.</exception>
    /// <exception cref="RateLimitException">This exception is thrown when a rate limit has been hit without the <see cref="FluxerRateLimitingHandler"/>.</exception>
    /// <seealso href="https://docs.fluxer.app/http-api/users/email-and-password/#verify-password-change-code"/>
    public Task<PasswordChangeVerifyResponse> VerifyAsync(
        string ticket,
        string code,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<PasswordChangeVerifyRequest, PasswordChangeVerifyResponse>(
        HttpMethod.Post,
        VerifyUrl,
        request: new PasswordChangeVerifyRequest(code, ticket),
        DtoJsonContext.Default.PasswordChangeVerifyRequest,
        DtoJsonContext.Default.PasswordChangeVerifyResponse,
        bucket: RateLimitDefaults.UserPasswordChangeVerify,
        cancellationToken: cancellationToken
    );

    /// <summary>
    /// Requests to complete the password change.
    /// </summary>
    /// <param name="request">The completion request object containing the parameters.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    /// <exception cref="RestApiException">
    /// This exception is thrown:
    /// <list type="bullet">
    /// <item>INVALID_OR_EXPIRED_TICKET - Password change ticket is outside the verified state.</item>
    /// <item>INVALID_PROOF_TOKEN - Proof token is invalid.</item>
    /// <item>PASSWORD_IS_TOO_COMMON - Password is in breached password corpus.</item>
    /// <item>TICKET_ALREADY_COMPLETED - Password change ticket is already in completed state.</item>
    /// </list>
    /// </exception>
    /// <exception cref="RateLimitException">This exception is thrown when a rate limit has been hit without the <see cref="FluxerRateLimitingHandler"/>.</exception>
    /// <seealso href="https://docs.fluxer.app/http-api/users/email-and-password/#complete-password-change"/>
    public Task CompleteAsync(
        PasswordChangeCompleteRequest request,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync(
        HttpMethod.Post,
        CompleteUrl,
        request,
        DtoJsonContext.Default.PasswordChangeCompleteRequest,
        bucket: RateLimitDefaults.UserPasswordChangeComplete,
        cancellationToken: cancellationToken
    );
}