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

namespace Fluxify.Dto.Users.Settings.EmailChange;

/// <summary>
/// Request object of the new email verification start request.
/// </summary>
/// <param name="Ticket">The ticket of the email change process.</param>
/// <param name="OriginalProof">Proof returned from the original email verification.</param>
/// <param name="NewEmail">The email address that the ticket should be bound to.</param>
/// <param name="NewPassword">This can be provided when claiming an account.</param>
/// <remarks>
/// Providing <paramref name="NewPassword"/> during account claim process will verify the password and might result in a PASSWORD_IS_TOO_COMMON api error.
/// </remarks>
/// <seealso href="https://docs.fluxer.app/http-api/users/email-and-password/#json-body-3"/>
public record EmailChangeRequestNewRequest(
    string OriginalProof,
    string Ticket,
    string NewEmail,
    string? NewPassword
);