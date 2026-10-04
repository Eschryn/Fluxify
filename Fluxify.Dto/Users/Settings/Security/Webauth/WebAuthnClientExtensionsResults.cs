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
/// Client extension outputs.
/// </summary>
/// <param name="AppId">Whether the AppID extension was used.</param>
/// <param name="CredentialProperties">Credential properties output.</param>
/// <param name="HmacCreateSecret">Whether the authenticator created an HMAC secret.</param>
/// <seealso href="https://docs.fluxer.app/http-api/users/mfa/#webauthn-client-extension-results-object"/>
public record WebAuthnClientExtensionsResults(
    [property: JsonPropertyName("appid")] bool? AppId,
    [property: JsonPropertyName("credProps")] WebAuthnCredentialProperties? CredentialProperties,
    [property: JsonPropertyName("hmacCreateSecret")] bool? HmacCreateSecret
);