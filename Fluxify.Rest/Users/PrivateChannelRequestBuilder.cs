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
using Fluxify.Dto.Channels;
using Fluxify.Dto.Channels.Text.Messages;
using Fluxify.Dto.Users;

namespace Fluxify.Rest.Users;

public class PrivateChannelRequestBuilder(HttpClient client)
{
    private static readonly IFormatProvider FormatProvider = CultureInfo.InvariantCulture;
    
    private const string ChannelsUrl = "users/@me/channels";
    private const string PreloadUrl = "users/@me/channels/preload";
    private static readonly CompositeFormat PinUrl = CompositeFormat.Parse("users/@me/channels/{0}/pin");
    
    public async Task<ChannelResponse[]?> ListAsync(CancellationToken cancellationToken = default) 
        => await client.JsonRequestAsync<ChannelResponse[]>(
            HttpMethod.Get,
            ChannelsUrl,
            cancellationToken: cancellationToken
        );

    public async Task<ChannelResponse?> CreateAsync(
        CreatePrivateChannelRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<CreatePrivateChannelRequest, ChannelResponse>(
        HttpMethod.Post,
        ChannelsUrl,
        request,
        cancellationToken: cancellationToken
    );

    public async Task<Dictionary<Snowflake, MessageResponse>?> PreloadMessagesAsync(
        PreloadMessagesRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<PreloadMessagesRequest, Dictionary<Snowflake, MessageResponse>>(
        HttpMethod.Post,
        PreloadUrl,
        request,
        cancellationToken: cancellationToken
    );

    public async Task PinChannelAsync(
        Snowflake channelId,
        CancellationToken cancellationToken = default
    ) => await client.RequestAsync(
        HttpMethod.Put,
        string.Format(FormatProvider, PinUrl, channelId),
        cancellationToken: cancellationToken
    );
    
    public async Task UnpinChannelAsync(
        Snowflake channelId,
        CancellationToken cancellationToken = default
    ) => await client.RequestAsync(
        HttpMethod.Delete,
        string.Format(FormatProvider, PinUrl, channelId),
        cancellationToken: cancellationToken
    );
}