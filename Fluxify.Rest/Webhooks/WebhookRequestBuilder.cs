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
using Fluxify.Core.Types;
using Fluxify.Dto.Webhooks;

namespace Fluxify.Rest.Webhooks;

public class WebhookRequestBuilder(HttpClient httpClient, Snowflake id)
{
    private static readonly IFormatProvider FormatProvider = CultureInfo.InvariantCulture;
    private static readonly CompositeFormat WebhookUrl = CompositeFormat.Parse("/webhooks/{0}");

    public Task<WebhookResponse> GetAsync(CancellationToken cancellationToken = default)
        => httpClient.JsonRequestAsync<WebhookResponse>(
            HttpMethod.Get,
            string.Format(FormatProvider, WebhookUrl, id),
            cancellationToken: cancellationToken
        );

    public Task DeleteAsync(CancellationToken cancellationToken = default)
        => httpClient.RequestAsync(
            HttpMethod.Delete,
            string.Format(FormatProvider, WebhookUrl, id),
            cancellationToken: cancellationToken
        );
    
    public Task<WebhookResponse> UpdateAsync(
        WebhookUpdateRequest request,
        CancellationToken cancellationToken = default
    ) => httpClient.JsonRequestAsync<WebhookUpdateRequest, WebhookResponse>(
        HttpMethod.Patch,
        string.Format(FormatProvider, WebhookUrl, id),
        request,
        cancellationToken: cancellationToken
    );
}