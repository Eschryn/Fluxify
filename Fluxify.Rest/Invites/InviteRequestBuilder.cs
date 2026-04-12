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
using Fluxify.Dto.Invites;

namespace Fluxify.Rest.Invites;

public class InviteRequestBuilder(HttpClient httpClient, string code)
{
    private static readonly IFormatProvider FormatProvider = CultureInfo.InvariantCulture;
    private static readonly CompositeFormat InviteUrl = CompositeFormat.Parse("invites/{0}");

    public Task<InviteMetadataResponseSchema> GetAsync(
        CancellationToken cancellationToken = default
    ) => httpClient.JsonRequestAsync<InviteMetadataResponseSchema>(
        HttpMethod.Get,
        string.Format(FormatProvider, InviteUrl, code),
        cancellationToken: cancellationToken
    );

    public Task<InviteResponseSchema> AcceptAsync(
        CancellationToken cancellationToken = default
    ) => httpClient.JsonRequestAsync<InviteResponseSchema>(
        HttpMethod.Post,
        string.Format(FormatProvider, InviteUrl, code),
        cancellationToken: cancellationToken
    );

    public Task DeleteAsync(
        CancellationToken cancellationToken = default
    ) => httpClient.RequestAsync(
        HttpMethod.Delete,
        string.Format(FormatProvider, InviteUrl, code),
        cancellationToken: cancellationToken
    );
}