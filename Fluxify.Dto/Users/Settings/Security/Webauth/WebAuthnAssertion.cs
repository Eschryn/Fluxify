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

using System.Text.Json.Serialization;

namespace Fluxify.Dto.Users.Settings.Security.Webauth;

/// <summary>
/// The webauthn assertion object.
/// </summary>
/// <param name="Id">Base64Url Credential id.</param>
/// <param name="RawId">Base64Url Raw credential id.</param>
/// <param name="Response">Authenticator assertion response.</param>
/// <param name="AuthenticatorAttachment">Authenticator attachment, either cross-platform or platform.</param>
/// <param name="ClientExtensionResults">Client extension outputs.</param>
/// <param name="Type">Always public-key.</param>
/// <seealso href="https://docs.fluxer.app/http-api/users/mfa/#webauthn-assertion-response-object"/>
public record WebAuthnAssertion(
    string Id,
    [property: JsonPropertyName("rawId")] string RawId,
    WebAuthnAssertionResponse Response,
    [property: JsonPropertyName("authenticatorAttachment")]  string? AuthenticatorAttachment,
    [property: JsonPropertyName("clientExtensionResults")] WebAuthnClientExtensionsResults ClientExtensionResults,
    string Type
);