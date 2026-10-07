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

namespace Fluxify.Dto.Users.Settings.PasswordChange;

/// <summary>
/// Response object that contains the verification proof returned from the verify request.
/// </summary>
/// <param name="VerificationProof">Verification proof that can be used to complete the password change.</param>
/// <seealso href="https://docs.fluxer.app/http-api/users/email-and-password/#password-verification-object"/>
public record PasswordChangeVerifyResponse(
    string VerificationProof
);