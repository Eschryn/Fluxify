using System.Globalization;
using System.Text;
using Fluxify.Core.Types;
using Fluxify.Dto.Users.ScheduledMessages;

namespace Fluxify.Rest.Users;

public class ScheduledMessagesRequestBuilder(HttpClient client)
{
    private static readonly IFormatProvider FormatProvider = CultureInfo.InvariantCulture;
    private const string ScheduledMessagesUrl = "users/@me/scheduled-messages";
    private static readonly CompositeFormat ScheduledMessageUrl = CompositeFormat.Parse("users/@me/scheduled-messages/{0}");
    
    public async Task<ScheduleMessageResponseSchema[]?> ListScheduledMessagesAsync(
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<ScheduleMessageResponseSchema[]>(
        HttpMethod.Get,
        ScheduledMessagesUrl,
        cancellationToken: cancellationToken
    );
    
    public async Task<ScheduleMessageResponseSchema?> GetScheduledMessageAsync(
        Snowflake messageId,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<ScheduleMessageResponseSchema>(
        HttpMethod.Get,
        string.Format(FormatProvider, ScheduledMessageUrl, messageId),
        cancellationToken: cancellationToken
    );
    
    public async Task DeleteScheduledMessageAsync(
        Snowflake messageId,
        CancellationToken cancellationToken = default
    ) => await client.RequestAsync(
        HttpMethod.Delete,
        string.Format(FormatProvider, ScheduledMessageUrl, messageId),
        cancellationToken: cancellationToken
    );
    
    public async Task<ScheduleMessageResponseSchema?> UpdateScheduledMessageAsync(
        UpdateScheduledMessageRequest request,
        CancellationToken cancellationToken = default
    ) => await client.MultipartJsonRequestAsync<ScheduledMessageResponseSchemaPayload, ScheduleMessageResponseSchema>(
        HttpMethod.Patch,
        string.Format(FormatProvider, ScheduledMessageUrl, request.Id),
        request,
        cancellationToken: cancellationToken
    );
}