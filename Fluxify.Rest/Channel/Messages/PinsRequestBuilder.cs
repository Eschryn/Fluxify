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
using Fluxify.Dto.Channels.Text.Messages.Pin;

namespace Fluxify.Rest.Channel.Messages;

public class PinsRequestBuilder(HttpClient client, Snowflake channelId)
{
    private static readonly CultureInfo FormatProvider = CultureInfo.InvariantCulture;
    private static readonly CompositeFormat GetUrl = CompositeFormat.Parse("channels/{0}/messages/pins");
    private static readonly CompositeFormat AckUrl = CompositeFormat.Parse("channels/{0}/pins/ack");
    private static string Uri(CompositeFormat format, Snowflake id) => string.Format(FormatProvider, format, id);
    
    public PinRequestBuilder this[Snowflake messageId] => new(client, channelId, messageId);

    public async Task<ChannelPinsResponse?> ListPinsAsync(int? limit = null, DateTimeOffset? before = null, CancellationToken cancellationToken = default)
        => await client.JsonRequestAsync<ChannelPinsResponse>(
            HttpMethod.Get,
            Uri(GetUrl, channelId) + new QueryBuilder()
                .AddQuery("limit", limit?.ToString())
                .AddQuery("before", before?.ToString("R")),
            cancellationToken: cancellationToken
        );
    
    public async Task AcknowledgeAsync(CancellationToken cancellationToken = default)
        => await client.RequestAsync(
            HttpMethod.Post,
            Uri(AckUrl, channelId),
            cancellationToken: cancellationToken
        );
}