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
using Fluxify.Dto.Channels.GroupDm;

namespace Fluxify.Rest.Channel;

public class CallRequestBuilder(HttpClient client, Snowflake id)
{
    private static readonly CultureInfo FormatProvider = CultureInfo.InvariantCulture;
    private static readonly CompositeFormat CallUrl = CompositeFormat.Parse("channels/{0}/call");
    private static readonly CompositeFormat EndCallUrl = CompositeFormat.Parse("channels/{0}/call/end");
    private static readonly CompositeFormat RingCallUrl = CompositeFormat.Parse("channels/{0}/call/ring");
    private static readonly CompositeFormat StopRingCallUrl = CompositeFormat.Parse("channels/{0}/call/stop-ringing");
    private static string Uri(CompositeFormat format, Snowflake id) => string.Format(FormatProvider, format, id);

    public async Task<CallEligibilityResponse?> GetEligibilityAsync(CancellationToken cancellationToken = default)
        => await client.JsonRequestAsync<CallEligibilityResponse>(
            HttpMethod.Get,
            Uri(CallUrl, id), 
            cancellationToken: cancellationToken
        );
    
    public async Task UpdateRegionAsync(UpdateCallRegionRequest request, CancellationToken cancellationToken = default)
        => await client.JsonRequestAsync<UpdateCallRegionRequest>(
            HttpMethod.Patch,
            Uri(CallUrl, id), 
            cancellationToken: cancellationToken
        );
    
    public async Task EndCallAsync(CancellationToken cancellationToken = default)
        => await client.RequestAsync(
            HttpMethod.Post,
            Uri(EndCallUrl, id), 
            cancellationToken: cancellationToken
        );
    
    public async Task RingAsync(RingRequest request, CancellationToken cancellationToken = default)
        => await client.JsonRequestAsync(
            HttpMethod.Post,
            Uri(RingCallUrl, id),
            request, 
            cancellationToken: cancellationToken
        );
    
    public async Task StopRingingAsync(RingRequest request, CancellationToken cancellationToken = default)
        => await client.JsonRequestAsync(
            HttpMethod.Post,
            Uri(StopRingCallUrl, id),
            request, 
            cancellationToken: cancellationToken
        );
}