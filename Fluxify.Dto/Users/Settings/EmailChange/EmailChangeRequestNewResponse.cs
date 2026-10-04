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
/// Response object for the new email set/change process request.
/// </summary>
/// <param name="NewCodeExpiresAt">From this moment on the code that was sent will expire, requiring a new one to be requested.</param>
/// <param name="NewEmail">The new email where the code was sent to.</param>
/// <param name="ResendAvailableAt">Timestamp from when on a new code can be requested to be sent to the new email.</param>
/// <param name="Ticket">The identifying ticket for this email change/set process.</param>
/// <seealso href="https://docs.fluxer.app/http-api/users/email-and-password/#new-email-request-object"/>
public record EmailChangeRequestNewResponse(
    DateTimeOffset NewCodeExpiresAt,
    string NewEmail,
    DateTimeOffset ResendAvailableAt,
    string Ticket
);