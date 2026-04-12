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

public class NoteRequestBuilder(HttpClient client, Snowflake userId)
{
    private static readonly IFormatProvider FormatProvider = CultureInfo.InvariantCulture;
    private static readonly CompositeFormat NoteUrl = CompositeFormat.Parse("users/@me/notes/{0}");
    
    public Task<UserNoteResponse> GetNoteAsync(
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<UserNoteResponse>(
        HttpMethod.Get,
        string.Format(FormatProvider, NoteUrl, userId),
        cancellationToken: cancellationToken
    );
    
    public Task SetNoteAsync(
        UserNoteUpdateRequest request,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync(
        HttpMethod.Put,
        string.Format(FormatProvider, NoteUrl, userId),
        request,
        cancellationToken: cancellationToken
    );
}