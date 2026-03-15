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

using Fluxify.Dto.Users.Settings.PasswordChange;

namespace Fluxify.Rest.Users;

public class PasswordChangeRequestBuilder(HttpClient client)
{
    private const string CompleteUrl = "users/@me/password-change/complete";
    private const string ResendUrl = "users/@me/password-change/resend";
    private const string StartUrl = "users/@me/password-change/start";
    private const string VerifyUrl = "users/@me/password-change/verify";
    
    public async Task CompleteAsync(
        PasswordChangeCompleteRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync(
        HttpMethod.Post,
        CompleteUrl,
        request,
        cancellationToken: cancellationToken
    );
    
    public async Task ResendAsync(
        PasswordChangeTicketRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync(
        HttpMethod.Post,
        ResendUrl,
        request,
        cancellationToken: cancellationToken
    );
    
    public async Task<PasswordChangeStartResponse?> StartAsync(
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<PasswordChangeStartResponse>(
        HttpMethod.Post,
        StartUrl,
        cancellationToken: cancellationToken
    );

    public async Task<PasswordChangeVerifyResponse?> VerifyAsync(
        PasswordChangeVerifyRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<PasswordChangeVerifyRequest, PasswordChangeVerifyResponse>(
        HttpMethod.Post,
        VerifyUrl,
        request,
        cancellationToken: cancellationToken
    );
}