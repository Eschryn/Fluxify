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

namespace Fluxify.Dto.Users.Settings.EmailChange;

/// <summary>
/// Response object for the email change start request.
/// </summary>
/// <param name="OriginalCodeExpiresAt">The moment the code, that was sent to the mail <paramref name="OriginalEmail"/>, expires.</param>
/// <param name="OriginalEmail">The current email address of the account. For unclaimed accounts null.</param>
/// <param name="OriginalProof">The proof for the original mail address. Is null when the original mail is unverified.</param>
/// <param name="RequireOriginal">Whether the original mail, if it exists requires a verification process so that the email can be changed.</param>
/// <param name="ResendAvailableAt">Earliest timestamp from which point can a new verification mail be requested.</param>
/// <param name="Ticket">This identifier is required for the entire email change process.</param>
/// <remarks>
/// For unclaimed accounts <paramref name="RequireOriginal"/> will be false, since the account doesn't have an original email yet.
/// In that case skip right to <see cref="EmailChangeRequestNewRequest"/> and use the <paramref name="OriginalProof"/> to start the verification process for the new email. 
/// <paramref name="RequireOriginal"/> is true when the account holds a verified email.
/// When <paramref name="RequireOriginal"/> is false, only then <paramref name="OriginalProof"/> will be set.
/// When <paramref name="RequireOriginal"/> is false, <paramref name="ResendAvailableAt"/> and <paramref name="OriginalCodeExpiresAt"/> will be null.
/// </remarks>
/// <seealso href="https://docs.fluxer.app/http-api/users/email-and-password/#email-change-start-object"/>
public record EmailChangeStartResponse(
    DateTimeOffset? OriginalCodeExpiresAt,
    string? OriginalEmail,
    string? OriginalProof,
    bool RequireOriginal,
    DateTimeOffset? ResendAvailableAt,
    string Ticket
);