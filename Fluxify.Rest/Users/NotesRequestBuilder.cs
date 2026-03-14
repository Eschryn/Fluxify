using Fluxify.Core.Types;

namespace Fluxify.Rest.Users;

public class NotesRequestBuilder(HttpClient client)
{
    private const string NotesUrl = "users/@me/notes";

    public NoteRequestBuilder this[Snowflake userId] => new(client, userId);
    
    public async Task<Dictionary<Snowflake, string>?> ListAllNotesAsync(
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<Dictionary<Snowflake, string>>(
        HttpMethod.Get,
        NotesUrl,
        cancellationToken: cancellationToken
    );
}