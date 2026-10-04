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
/// Request object for the verify replacement email for bounced address request.
/// </summary>
/// <param name="Code">The code that was sent to the users replacement email.</param>
/// <param name="Ticket">The ticket of the bounced email change process.</param>
/// <seealso href="https://docs.fluxer.app/http-api/users/email-and-password/#verify-replacement-email-for-bounced-address"/>
public record EmailChangeBouncedRequestVerifyNewRequest(string Code, string Ticket);