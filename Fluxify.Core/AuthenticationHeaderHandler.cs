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

namespace Fluxify.Core;

public class AuthenticationHeaderHandler( 
    FluxerConfig config
) : DelegatingHandler
{
    private string? _cachedTokenValue;
    
    protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken) 
        => throw new NotImplementedException();

    private async Task AddAuthorizationHeader(HttpRequestMessage request)
    {
        if (_cachedTokenValue is not null)
        {
            request.Headers.Add("Authorization", _cachedTokenValue);
            return;
        }
        
        if (await config.CredentialProvider() is { Token : { } token } credentials 
            && !string.IsNullOrEmpty(credentials.Token))
        {
            request.Headers.Add("Authorization", _cachedTokenValue = credentials.GetAuthorizationHeaderValue());
        }
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        await AddAuthorizationHeader(request);
        
        return await base.SendAsync(request, cancellationToken);
    }
}