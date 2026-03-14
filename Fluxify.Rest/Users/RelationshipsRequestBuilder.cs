using Fluxify.Core.Types;
using Fluxify.Dto.Users.Relationships;

namespace Fluxify.Rest.Users;

public class RelationshipsRequestBuilder(HttpClient client)
{
    private const string RelationshipsUrl = "users/@me/relationships";
    
    public RelationshipRequestBuilder this[Snowflake userId] => new(client, userId);
    
    public async Task<RelationshipResponse[]?> GetRelationshipsAsync(
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<RelationshipResponse[]>(
        HttpMethod.Get,
        RelationshipsUrl,
        cancellationToken: cancellationToken
    );
    
    public async Task<RelationshipResponse?> SendFriendRequestByTagAsync(
        FriendRequestByTagRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<FriendRequestByTagRequest, RelationshipResponse>(
        HttpMethod.Post,
        RelationshipsUrl,
        request,
        cancellationToken: cancellationToken
    );
}