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