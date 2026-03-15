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

using Fluxify.Dto.Users;
using Fluxify.Dto.Users.Settings.EmailChange;

namespace Fluxify.Rest.Users;

public class EmailChangeRequestBuilder(HttpClient client)
{
    private const string BouncedRequestNewUrl = "users/@me/email-change/bounced/request-new";
    private const string BouncedResendNewUrl = "users/@me/email-change/bounced/resend-new";
    private const string BouncedVerifyNewUrl = "users/@me/email-change/bounced/verify-new";
    private const string RequestNewUrl = "users/@me/email-change/request-new";
    private const string ResendNewUrl = "users/@me/email-change/resend-new";
    private const string ResendOriginalUrl = "users/@me/email-change/resend-original";
    private const string StartUrl = "users/@me/email-change/start";
    private const string VerifyNewUrl = "users/@me/email-change/verify-new";
    private const string VerifyOriginalUrl = "users/@me/email-change/verify-original";
    
    public async Task<EmailChangeRequestNewResponse?> BouncedRequestNewAsync(
        EmailChangeBouncedRequestNewRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<EmailChangeBouncedRequestNewRequest, EmailChangeRequestNewResponse>(
        HttpMethod.Post,
        BouncedRequestNewUrl,
        request,
        cancellationToken: cancellationToken
    );
    
    public async Task BouncedResendNewAsync(
        EmailChangeTicketRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync(
        HttpMethod.Post,
        BouncedResendNewUrl,
        request,
        cancellationToken: cancellationToken
    );
    
    public async Task<UserPrivate?> BouncedVerifyNewAsync(
        EmailChangeBouncedRequestVerifyNewRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<EmailChangeBouncedRequestVerifyNewRequest, UserPrivate>(
        HttpMethod.Post,
        BouncedVerifyNewUrl,
        request,
        cancellationToken: cancellationToken
    );
    
    public async Task<EmailChangeRequestNewResponse?> RequestNewAsync(
        EmailChangeRequestNewRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<EmailChangeRequestNewRequest, EmailChangeRequestNewResponse>(
        HttpMethod.Post,
        RequestNewUrl,
        request,
        cancellationToken: cancellationToken
    );
    
    public async Task ResendNewAsync(
        EmailChangeTicketRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync(
        HttpMethod.Post,
        ResendNewUrl,
        request,
        cancellationToken: cancellationToken
    );
    
    public async Task ResendOriginalAsync(
        EmailChangeTicketRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync(
        HttpMethod.Post,
        ResendOriginalUrl,
        request,
        cancellationToken: cancellationToken
    );
    
    public async Task<EmailChangeStartResponse?> StartAsync(
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<EmailChangeStartResponse>(
        HttpMethod.Post,
        StartUrl,
        cancellationToken: cancellationToken
    );

    public async Task<EmailTokenResponse?> VerifyNewAsync(
        EmailChangeVerifyNewRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<EmailChangeVerifyNewRequest, EmailTokenResponse>(
        HttpMethod.Post,
        VerifyNewUrl,
        request,
        cancellationToken: cancellationToken
    );

    public async Task<EmailChangeVerifyOriginalResponse?> VerifyOriginalAsync(
        EmailChangeVerifyOriginalRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<EmailChangeVerifyOriginalRequest, EmailChangeVerifyOriginalResponse>(
        HttpMethod.Post,
        VerifyOriginalUrl,
        request,
        cancellationToken: cancellationToken
    );
}