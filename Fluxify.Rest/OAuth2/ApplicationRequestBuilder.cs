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

using System.Globalization;
using System.Text;
using Fluxify.Dto.OAuth2;
using Fluxify.Dto.Users.Settings.Security;

namespace Fluxify.Rest.OAuth2;

public class ApplicationRequestBuilder(HttpClient httpClient, string applicationId)
{
    private static readonly CompositeFormat ApplicationUrl = CompositeFormat.Parse("oauth2/applications/{0}");
    private static readonly CompositeFormat BotUrl = CompositeFormat.Parse("oauth2/applications/{0}/bot");
    private static readonly CompositeFormat ResetTokenUrl = CompositeFormat.Parse("oauth2/applications/{0}/bot/reset-token");
    private static readonly CompositeFormat ResetSecretUrl = CompositeFormat.Parse("oauth2/applications/{0}/client-secret/reset");
    private static readonly CompositeFormat PublicApplicationUrl = CompositeFormat.Parse("oauth2/applications/{0}/public");

    public async Task<ApplicationResponse?> GetApplicationAsync(
        CancellationToken cancellationToken = default
    ) => await httpClient.JsonRequestAsync<ApplicationResponse>(HttpMethod.Get,
        string.Format(CultureInfo.InvariantCulture, ApplicationUrl, applicationId),
        cancellationToken: cancellationToken
    );

    public async Task DeleteApplication(
        SudoVerificationSchema request,
        CancellationToken cancellationToken = default
    ) => await httpClient.JsonRequestAsync(HttpMethod.Delete,
        string.Format(CultureInfo.InvariantCulture, ApplicationUrl, applicationId),
        request,
        cancellationToken: cancellationToken
    );

    public async Task<ApplicationResponse?> UpdateApplication(
        ApplicationUpdateRequest request,
        CancellationToken cancellationToken = default
    ) => await httpClient.JsonRequestAsync<ApplicationUpdateRequest, ApplicationResponse>(HttpMethod.Patch,
        string.Format(CultureInfo.InvariantCulture, ApplicationUrl, applicationId),
        request,
        cancellationToken: cancellationToken
    );

    public async Task<BotProfileResponse?> UpdateBotAsync(
        BotProfileUpdateRequest request,
        CancellationToken cancellationToken = default
    ) => await httpClient.JsonRequestAsync<BotProfileUpdateRequest, BotProfileResponse>(
        HttpMethod.Patch,
        string.Format(CultureInfo.InvariantCulture, BotUrl, applicationId),
        request,
        cancellationToken: cancellationToken
    );

    public async Task<BotTokenResetResponse?> ResetBotTokenAsync(
        SudoVerificationSchema request,
        CancellationToken cancellationToken = default
    ) => await httpClient.JsonRequestAsync<SudoVerificationSchema, BotTokenResetResponse>(
        HttpMethod.Post,
        string.Format(CultureInfo.InvariantCulture, ResetTokenUrl, applicationId),
        request,
        cancellationToken: cancellationToken
    );
    
    public async Task<ApplicationResponse?> ResetClientSecretAsync(
        SudoVerificationSchema request,
        CancellationToken cancellationToken = default
    ) => await httpClient.JsonRequestAsync<SudoVerificationSchema, ApplicationResponse>(
        HttpMethod.Post,
        string.Format(CultureInfo.InvariantCulture, ResetSecretUrl, applicationId),
        request,
        cancellationToken: cancellationToken
    );
    
    public async Task<ApplicationPublicResponse?> GetPublicApplicationAsync(
        CancellationToken cancellationToken = default
    ) => await httpClient.JsonRequestAsync<ApplicationPublicResponse>(
        HttpMethod.Get,
        string.Format(CultureInfo.InvariantCulture, PublicApplicationUrl, applicationId),
        cancellationToken: cancellationToken
    );
    
}