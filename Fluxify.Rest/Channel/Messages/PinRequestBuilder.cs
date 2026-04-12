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

namespace Fluxify.Rest.Channel.Messages;

public class PinRequestBuilder(HttpClient client, Snowflake channelId, Snowflake messageId)
{
    private static readonly CultureInfo FormatProvider = CultureInfo.InvariantCulture;
    private static readonly CompositeFormat GetUrl = CompositeFormat.Parse("channels/{0}/pins/{1}");

    private static string Uri(CompositeFormat format, Snowflake channelId, Snowflake messageId)
        => string.Format(FormatProvider, format, channelId, messageId);

    public Task PinAsync(CancellationToken cancellationToken = default)
        => client.RequestAsync(
            HttpMethod.Put, 
            Uri(GetUrl, channelId, messageId),
            cancellationToken: cancellationToken
        );

    public Task UnpinAsync(CancellationToken cancellationToken = default)
        => client.RequestAsync(
            HttpMethod.Delete, 
            Uri(GetUrl, channelId, messageId),
            cancellationToken: cancellationToken
        );
}