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
    protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        AddAuthorizationHeader(request);

        return base.Send(request, cancellationToken);
    }

    private void AddAuthorizationHeader(HttpRequestMessage request)
    {
        if (config.Credentials is { Token: { } token } credentials 
            && !string.IsNullOrEmpty(credentials.Token))
        {
            request.Headers.Add("Authorization", credentials.GetAuthorizationHeaderValue());
        }
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        AddAuthorizationHeader(request);
        
        return base.SendAsync(request, cancellationToken);
    }
}