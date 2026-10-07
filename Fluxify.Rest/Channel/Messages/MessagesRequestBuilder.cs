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
using Fluxify.Dto.Channels.Text.Messages.BulkDelete;

namespace Fluxify.Rest.Channel.Messages;

public class MessagesRequestBuilder(HttpClient client, Snowflake channelId)
{
    private static readonly CultureInfo FormatProvider = CultureInfo.InvariantCulture;
    private static readonly CompositeFormat GetUrl = CompositeFormat.Parse("channels/{0}/messages");
    private static readonly CompositeFormat AckUrl = CompositeFormat.Parse("channels/{0}/messages/ack");
    private static readonly CompositeFormat BulkDeleteUrl = CompositeFormat.Parse("channels/{0}/messages/bulk-delete");
    private static string Uri(CompositeFormat format, Snowflake id) => string.Format(FormatProvider, format, id);

    public MessageRequestBuilder this[Snowflake messageId] => new(client, channelId, messageId);

    public PinsRequestBuilder Pins { get; } = new(client, channelId);

    public Task<MessageResponse> SendMessageAsync(CreateMessageRequest createRequest,
        CancellationToken cancellationToken = default)
        => client.MultipartJsonRequestAsync<CreateMessageRequest, MessageResponse>(
            HttpMethod.Post,
            Uri(GetUrl, channelId),
            createRequest,
            DtoJsonContext.Default.CreateMessageRequest,
            DtoJsonContext.Default.MessageResponse,
            cancellationToken: cancellationToken
        );

    public Task<MessageResponse[]> ListMessagesAsync(
        int? limit = null,
        Snowflake? before = null,
        Snowflake? after = null,
        Snowflake? around = null,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<MessageResponse[]>(
        HttpMethod.Get,
        Uri(GetUrl, channelId) + new QueryBuilder()
            .AddQuery("limit", limit?.ToString(FormatProvider))
            .AddQuery("before", before?.ToString(FormatProvider))
            .AddQuery("after", after?.ToString(FormatProvider))
            .AddQuery("around", around?.ToString(FormatProvider)),
        DtoJsonContext.Default.MessageResponseArray,
        cancellationToken: cancellationToken
    );

    public Task MarkUnreadAsync(
        CancellationToken cancellationToken = default
    ) => client.RequestAsync(HttpMethod.Delete, Uri(AckUrl, channelId), cancellationToken: cancellationToken);

    public Task BulkDeleteAsync(
        BulkDeleteMessagesRequest request,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync(
        HttpMethod.Post,
        Uri(BulkDeleteUrl, channelId),
        request,
        DtoJsonContext.Default.BulkDeleteMessagesRequest,
        cancellationToken: cancellationToken
    );
}