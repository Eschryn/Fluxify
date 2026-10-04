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

using Fluxify.Dto.Users.Settings.Security.Webauth;

namespace Fluxify.Dto.Users.Settings.Security;

/// <summary>
/// Base verification schema for many authenticated calls.
/// </summary>
/// <param name="MfaCode">MFA Code as received by the authenticator.</param>
/// <param name="MfaMethod">Which method should be used for authentication.</param>
/// <param name="Password">Password of the account.</param>
/// <param name="WebauthnChallenge">WebAuthn challenge.</param>
/// <param name="WebauthnResponse">WebAuthn response.</param>
public record SudoVerificationSchema(
    string? MfaCode,
    MfaMethod? MfaMethod,
    string? Password,
    string? WebauthnChallenge,
    WebAuthnAssertion? WebauthnResponse
);