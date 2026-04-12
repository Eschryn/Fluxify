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

using Fluxify.Dto.Common;
using Fluxify.Dto.Users.Settings.Security;

namespace Fluxify.Rest.Users;

public class UserMessagesRequestBuilder(HttpClient client)
{
    private const string DeleteUrl = "users/@me/messages/delete";
    private const string DeleteTestUrl = "users/@me/messages/delete/test";

    public Task DeleteBulkAsync(
        SudoVerificationSchema request,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync(
        HttpMethod.Post,
        DeleteUrl,
        request,
        cancellationToken: cancellationToken
    );
    
    public async Task<bool> CancelBulkDeleteAsync(
        CancellationToken cancellationToken = default
    ) => (await client.JsonRequestAsync<SuccessResponse>(
        HttpMethod.Delete,
        DeleteUrl,
        cancellationToken: cancellationToken
    )).Success;
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <remarks>This endpoint is staff only!</remarks>
    public Task TestDeleteBulkAsync(
        CancellationToken cancellationToken = default
    ) => client.RequestAsync(
        HttpMethod.Post,
        DeleteTestUrl,
        cancellationToken: cancellationToken
    );
}