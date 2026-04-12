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
using Fluxify.Dto.Users.Relationships;

namespace Fluxify.Rest.Users;

public class RelationshipRequestBuilder(HttpClient client, Snowflake userId)
{
    private static readonly IFormatProvider FormatProvider = CultureInfo.InvariantCulture;
    private static readonly CompositeFormat RelationshipUrl = CompositeFormat.Parse("users/@me/relationships/{0}");
    
    public Task<RelationshipResponse> UpdateFriendRequestAsync(
        RelationshipTypePutRequest request,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<RelationshipTypePutRequest, RelationshipResponse>(
        HttpMethod.Put,
        string.Format(FormatProvider, RelationshipUrl, userId),
        request,
        cancellationToken: cancellationToken
    );
    
    public Task<RelationshipResponse> SendFriendRequestAsync(
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<RelationshipResponse>(
        HttpMethod.Post,
        string.Format(FormatProvider, RelationshipUrl, userId),
        cancellationToken: cancellationToken
    );

    public Task RemoveAsync(
        CancellationToken cancellationToken = default
    ) => client.RequestAsync(
        HttpMethod.Delete,
        string.Format(FormatProvider, RelationshipUrl, userId),
        cancellationToken: cancellationToken
    );
    
    public Task<RelationshipResponse> SetNicknameAsync(
        RelationshipNicknameUpdateRequest request,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<RelationshipNicknameUpdateRequest, RelationshipResponse>(
        HttpMethod.Patch,
        string.Format(FormatProvider, RelationshipUrl, userId),
        request,
        cancellationToken: cancellationToken
    );
}