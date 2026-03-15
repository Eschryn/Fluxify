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
using Fluxify.Dto.Channels.Text.Messages;
using Fluxify.Rest.Channel.Messages;

namespace Fluxify.Rest.Channel;

public class MessageRequestBuilder(HttpClient client, Snowflake channelId, Snowflake messageId)
{
    private static readonly CultureInfo FormatProvider = CultureInfo.InvariantCulture;
    private static readonly CompositeFormat GetUrl = CompositeFormat.Parse("channels/{0}/messages/{1}");
    private static readonly CompositeFormat AckUrl = CompositeFormat.Parse("channels/{0}/messages/{1}/ack");
    private static readonly CompositeFormat AttachmentUrl = CompositeFormat.Parse("channels/{0}/messages/{1}/attachment/{2}");
    private static string Uri(CompositeFormat format, Snowflake channelId, Snowflake messageId) 
        => string.Format(FormatProvider, format, channelId, messageId);
    
    public MessageReactionsRequestBuilder Reactions => new(client, channelId, messageId);
    
    public async Task<MessageResponse?> GetMessageAsync(CancellationToken cancellationToken = default)
        => await client.JsonRequestAsync<MessageResponse>(
            HttpMethod.Get,
            Uri(GetUrl, channelId, messageId),
            cancellationToken: cancellationToken
        );
    
    public async Task DeleteMessageAsync(CancellationToken cancellationToken = default)
        => await client.RequestAsync(
            HttpMethod.Delete,
            Uri(GetUrl, channelId, messageId),
            cancellationToken: cancellationToken
        );
    
    public async Task<MessageResponse?> UpdateMessageAsync(UpdateMessageRequest request, CancellationToken cancellationToken = default)
        => await client.JsonRequestAsync<UpdateMessageRequest, MessageResponse>(
            HttpMethod.Patch,
            Uri(GetUrl, channelId, messageId),
            request,
            cancellationToken: cancellationToken
        );
    
    public async Task AckMessageAsync(CancellationToken cancellationToken = default)
        => await client.RequestAsync(
            HttpMethod.Post,
            Uri(AckUrl, channelId, messageId),
            cancellationToken: cancellationToken
        );
    
    public async Task DeleteAttachmentAsync(Snowflake attachmentId, CancellationToken cancellationToken = default)
        => await client.RequestAsync(
            HttpMethod.Delete,
            string.Format(FormatProvider, AttachmentUrl, channelId, messageId, attachmentId),
            cancellationToken: cancellationToken
        );
}