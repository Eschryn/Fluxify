using System.Globalization;
using System.Text;
using Fluxify.Core.Types;
using Fluxify.Dto.Channels.Text.Messages;
using Fluxify.Dto.Channels.Text.Messages.BulkDelete;
using Fluxify.Dto.Users.ScheduledMessages;

namespace Fluxify.Rest.Channel;

public class MessagesRequestBuilder(HttpClient client, Snowflake channelId)
{
    private static readonly CultureInfo FormatProvider = CultureInfo.InvariantCulture;
    private static readonly CompositeFormat GetUrl = CompositeFormat.Parse("channels/{0}/messages");
    private static readonly CompositeFormat AckUrl = CompositeFormat.Parse("channels/{0}/messages/ack");
    private static readonly CompositeFormat BulkDeleteUrl = CompositeFormat.Parse("channels/{0}/messages/bulk-delete");
    private static readonly CompositeFormat ScheduleUrl = CompositeFormat.Parse("channels/{0}/messages/schedule");
    private static string Uri(CompositeFormat format, Snowflake id) => string.Format(FormatProvider, format, id);

    public MessageRequestBuilder this[Snowflake messageId] => new(client, channelId, messageId);
    
    public PinsRequestBuilder Pins => new(client, channelId);
    
    public async Task<MessageResponse?> SendMessageAsync(CreateMessageRequest createRequest,
        CancellationToken cancellationToken = default)
        => await client.MultipartJsonRequestAsync<CreateMessageRequest, MessageResponse>(
            HttpMethod.Post,
            Uri(GetUrl, channelId),
            createRequest,
            cancellationToken: cancellationToken
        );

    public async Task<MessageResponse[]?> ListMessagesAsync(
        int? limit = null,
        Snowflake? before = null,
        Snowflake? after = null,
        Snowflake? around = null,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<MessageResponse[]>(
        HttpMethod.Get,
        Uri(GetUrl, channelId) + new QueryBuilder()
            .AddQuery("limit", limit?.ToString(FormatProvider))
            .AddQuery("before", before?.ToString(FormatProvider))
            .AddQuery("after", after?.ToString(FormatProvider))
            .AddQuery("around", around?.ToString(FormatProvider)),
        cancellationToken: cancellationToken
    );

    public async Task MarkUnreadAsync() => await client.RequestAsync(HttpMethod.Delete, Uri(AckUrl, channelId));
    public async Task BulkDeleteAsync(BulkDeleteMessagesRequest request) 
        => await client.JsonRequestAsync(HttpMethod.Post, Uri(BulkDeleteUrl, channelId), request);
    
    public async Task<ScheduleMessageResponseSchema?> ScheduleMessageAsync(
        ScheduledMessageResponseSchemaPayload request,
        CancellationToken cancellationToken = default
    ) => await client.MultipartJsonRequestAsync<ScheduledMessageResponseSchemaPayload, ScheduleMessageResponseSchema>(
            HttpMethod.Post,
            Uri(ScheduleUrl, channelId),
            request,
            cancellationToken: cancellationToken
        );
}