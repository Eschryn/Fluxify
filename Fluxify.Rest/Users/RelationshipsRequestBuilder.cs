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