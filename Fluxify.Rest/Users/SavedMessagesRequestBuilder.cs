using System.Globalization;
using System.Text;
using Fluxify.Core.Types;
using Fluxify.Dto.Users;
using Fluxify.Rest.Channel;

namespace Fluxify.Rest.Users;

public class SavedMessagesRequestBuilder(HttpClient client)
{
    private static readonly IFormatProvider FormatProvider = CultureInfo.InvariantCulture;
    
    private const string SavedMessagesUrl = "users/@me/saved-messages";
    private static readonly CompositeFormat SavedMessageUrl = CompositeFormat.Parse("users/@me/saved-messages/{0}");
    
    public async Task<SavedMessageEntryResponse[]?> GetSavedMessagesAsync(
        int? limit = null,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<SavedMessageEntryResponse[]>(
        HttpMethod.Get,
        SavedMessagesUrl + new QueryBuilder()
            .AddQuery("limit", limit?.ToString()),
        cancellationToken: cancellationToken
    );

    public async Task SaveMessageAsync(
        SaveMessageRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync(
        HttpMethod.Post,
        SavedMessagesUrl,
        request,
        cancellationToken: cancellationToken
    );
    
    public async Task UnsaveMessageAsync(
        Snowflake messageId,
        CancellationToken cancellationToken = default
    ) => await client.RequestAsync(
        HttpMethod.Delete,
        string.Format(FormatProvider, SavedMessageUrl, messageId),
        cancellationToken: cancellationToken
    );
}