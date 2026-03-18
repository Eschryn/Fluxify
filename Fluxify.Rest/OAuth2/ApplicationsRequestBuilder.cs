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

using Fluxify.Dto.OAuth2;

namespace Fluxify.Rest.OAuth2;

public class ApplicationsRequestBuilder(HttpClient httpClient)
{
    private const string ApplicationsCreateUrl = "oauth2/applications";
    private const string ApplicationsUrl = "applications/@me";
    private const string UserApplicationsUrl = "oauth2/applications/@me";
    public ApplicationRequestBuilder this[string applicationId] => new(httpClient, applicationId);
    
    public async Task<ApplicationResponse?> CreateApplicationAsync(
        ApplicationCreateRequest request,
        CancellationToken cancellationToken = default
    ) => await httpClient.JsonRequestAsync<ApplicationCreateRequest, ApplicationResponse>(
        HttpMethod.Post,
        ApplicationsCreateUrl,
        request,
        cancellationToken: cancellationToken
    );

    public async Task<ApplicationsMeResponse[]?> ListUserApplicationsAsync(
        CancellationToken cancellationToken = default
    ) => await httpClient.JsonRequestAsync<ApplicationsMeResponse[]>(
        HttpMethod.Get,
        UserApplicationsUrl,
        cancellationToken: cancellationToken
    );
    
    
    public async Task<OAuth2MeResponseApplication[]?> ListApplicationsAsync(
        CancellationToken cancellationToken = default)
        => await httpClient.JsonRequestAsync<OAuth2MeResponseApplication[]>(
            HttpMethod.Get,
            ApplicationsUrl,
            cancellationToken: cancellationToken
        );
}