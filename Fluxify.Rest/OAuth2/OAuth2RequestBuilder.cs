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

namespace Fluxify.Rest.OAuth2;

public class OAuth2RequestBuilder(HttpClient httpClient)
{
    private const string MeUrl = "oauth2/@me";
    private const string AuthorizationsUrl = "oauth2/authorizations/@me";
    private static readonly CompositeFormat AuthorizationUrl = CompositeFormat.Parse("oauth2/authorizations/{0}");
    
    public ApplicationsRequestBuilder Applications { get; } = new(httpClient);

    public async Task<OAuth2MeResponse?> MeAsync(CancellationToken cancellationToken = default)
        => await httpClient.JsonRequestAsync<OAuth2MeResponse>(
            HttpMethod.Get,
            MeUrl,
            cancellationToken: cancellationToken
        );

    public async Task<OAuth2AuthorizationResponse[]?> ListAuthorizationsAsync(
        CancellationToken cancellationToken = default)
        => await httpClient.JsonRequestAsync<OAuth2AuthorizationResponse[]>(
            HttpMethod.Get,
            AuthorizationsUrl,
            cancellationToken: cancellationToken
        );

    public async Task RevokeAuthorizationAsync(string applicationId, CancellationToken cancellationToken = default)
        => await httpClient.JsonRequestAsync<OAuth2AuthorizationResponse>(
            HttpMethod.Delete,
            string.Format(CultureInfo.InvariantCulture, AuthorizationUrl, applicationId),
            cancellationToken: cancellationToken
        );
}