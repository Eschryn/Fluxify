using System.Globalization;
using System.Text;
using Fluxify.Core.Types;
using Fluxify.Dto.Users.Relationships;

namespace Fluxify.Rest.Users;

public class RelationshipRequestBuilder(HttpClient client, Snowflake userId)
{
    private static readonly IFormatProvider FormatProvider = CultureInfo.InvariantCulture;
    private static readonly CompositeFormat RelationshipUrl = CompositeFormat.Parse("users/@me/relationships/{0}");
    
    public async Task<RelationshipResponse?> UpdateFriendRequestAsync(
        RelationshipTypePutRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<RelationshipTypePutRequest, RelationshipResponse>(
        HttpMethod.Put,
        string.Format(FormatProvider, RelationshipUrl, userId),
        request,
        cancellationToken: cancellationToken
    );
    
    public async Task<RelationshipResponse?> SendFriendRequestAsync(
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<RelationshipResponse>(
        HttpMethod.Post,
        string.Format(FormatProvider, RelationshipUrl, userId),
        cancellationToken: cancellationToken
    );

    public async Task RemoveAsync(
        CancellationToken cancellationToken = default
    ) => await client.RequestAsync(
        HttpMethod.Delete,
        string.Format(FormatProvider, RelationshipUrl, userId),
        cancellationToken: cancellationToken
    );
    
    public async Task<RelationshipResponse?> SetNicknameAsync(
        RelationshipNicknameUpdateRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<RelationshipNicknameUpdateRequest, RelationshipResponse>(
        HttpMethod.Patch,
        string.Format(FormatProvider, RelationshipUrl, userId),
        request,
        cancellationToken: cancellationToken
    );
}