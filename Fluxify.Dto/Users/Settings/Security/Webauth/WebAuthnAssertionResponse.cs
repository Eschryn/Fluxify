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
/// Contains authenticator assertion response.
/// </summary>
/// <param name="ClientDataJson">Base64Url: JSON compatible serialization of client data </param>
/// <param name="AuthenticationData">Base64Url: encodes the contextual bindings made by the authenticator <see href="https://www.w3.org/TR/webauthn-2/#authenticator-data"/></param>
/// <param name="Signature">Base64Url: signature returned by the authenticator.</param>
/// <param name="UserHandle">Base64Url: user handle.</param>
/// <seealso href="https://docs.fluxer.app/http-api/users/mfa/#webauthn-assertion-response-object"/>
/// <seealso href="https://www.w3.org/TR/webauthn-2/#sctn-discover-from-external-source"/>
public record WebAuthnAssertionResponse(
    [property: JsonPropertyName("clientDataJSON")]
    string ClientDataJson,
    [property: JsonPropertyName("authenticationData")]
    string AuthenticationData,
    string Signature,
    [property: JsonPropertyName("userHandle")]
    string? UserHandle
);