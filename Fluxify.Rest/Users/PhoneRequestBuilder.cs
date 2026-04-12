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

using Fluxify.Dto.Users.Settings.PhoneChange;
using Fluxify.Dto.Users.Settings.Security;

namespace Fluxify.Rest.Users;

public class PhoneRequestBuilder(HttpClient client)
{
    private const string PhoneUrl = "users/@me/phone";
    private const string SendVerificationUrl = "users/@me/phone/send-verification";
    private const string VerifyUrl = "users/@me/phone/verify";
    
    public Task AddPhoneNumberAsync(
        PhoneAddRequest request,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync(
        HttpMethod.Post,
        PhoneUrl,
        request,
        cancellationToken: cancellationToken
    );
    
    public Task DeletePhoneNumberAsync(
        SudoVerificationSchema request,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync(
        HttpMethod.Delete,
        PhoneUrl,
        request,
        cancellationToken: cancellationToken
    );
    
    public Task SendVerificationAsync(
        PhoneSendVerificationRequest request,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync(
        HttpMethod.Post,
        SendVerificationUrl,
        request,
        cancellationToken: cancellationToken
    );
    
    public Task<PhoneVerifyResponse> VerifyAsync(
        PhoneVerifyRequest request,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<PhoneVerifyRequest, PhoneVerifyResponse>(
        HttpMethod.Post,
        VerifyUrl,
        request,
        cancellationToken: cancellationToken
    );
}