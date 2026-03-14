using System.Globalization;
using System.Text;
using Fluxify.Core.Types;
using Fluxify.Dto.Users.Relationships;

namespace Fluxify.Rest.Users;

public class NoteRequestBuilder(HttpClient client, Snowflake userId)
{
    private static readonly IFormatProvider FormatProvider = CultureInfo.InvariantCulture;
    private static readonly CompositeFormat NoteUrl = CompositeFormat.Parse("users/@me/notes/{0}");
    
    public async Task<UserNoteResponse?> GetNoteAsync(
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<UserNoteResponse>(
        HttpMethod.Get,
        string.Format(FormatProvider, NoteUrl, userId),
        cancellationToken: cancellationToken
    );
    
    public async Task SetNoteAsync(
        UserNoteUpdateRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync(
        HttpMethod.Put,
        string.Format(FormatProvider, NoteUrl, userId),
        request,
        cancellationToken: cancellationToken
    );
}