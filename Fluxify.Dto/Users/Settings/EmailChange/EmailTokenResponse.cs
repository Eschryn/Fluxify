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
/// The response object of the new email verification request.
/// </summary>
/// <param name="EmailToken">The token that can be redeemed at the apply email change request or for claiming an account at the modify user request.</param>
/// <seealso href="https://docs.fluxer.app/http-api/users/email-and-password/#new-email-verification-object"/>
public record EmailTokenResponse(string EmailToken);