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

using Fluxify.Dto.Users.Settings.Security.Mfa;
using Fluxify.Dto.Users.Settings.Security.Webauth;

namespace Fluxify.Rest.Users;

public class SudoRequestBuilder(HttpClient client)
{
    private const string MfaMethodsUrl = "users/@me/sudo/mfa-methods";
    private const string SmsSendUrl = "users/@me/sudo/sms/send";
    private const string WebAuthnOptionsUrl = "users/@me/sudo/webauthn/authentication-options";
    
    public async Task<SudoMfaMethodsResponse?> GetMfaMethodsAsync(CancellationToken cancellationToken = default)
        => await client.JsonRequestAsync<SudoMfaMethodsResponse>(
            HttpMethod.Get,
            MfaMethodsUrl,
            cancellationToken: cancellationToken
        );
    
    public async Task SendSmsCodeAsync(
        CancellationToken cancellationToken = default
    ) => await client.RequestAsync(
        HttpMethod.Post,
        SmsSendUrl,
        cancellationToken: cancellationToken
    );
    
    public async Task<WebAuthnChallengeResponse?> GetWebAuthnOptionsAsync(
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<WebAuthnChallengeResponse>(
        HttpMethod.Post,
        WebAuthnOptionsUrl,
        cancellationToken: cancellationToken
    );
}