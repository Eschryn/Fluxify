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

using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Fluxify.Core;
using Fluxify.Dto.Json;
using Fluxify.Rest.Channel;
using Fluxify.Rest.Guilds;
using Fluxify.Rest.Invites;
using Fluxify.Rest.Model;
using Fluxify.Rest.OAuth2;
using Fluxify.Rest.Packs;
using Fluxify.Rest.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Fluxify.Rest;

public class RestClient
{
    private readonly FluxerConfig _config;
    private readonly HttpClient _httpClient;

    public RestClient(FluxerConfig config)
    {
        _config = config;
        _httpClient = config.ServiceProvider.GetRequiredService<HttpClient>();

        Guilds = new GuildsRequestBuilder(_httpClient);
        Users = new UsersRequestBuilder(_httpClient);
        Channels = new ChannelsRequestBuilder(_httpClient);
        Gateway = new GatewayRequestBuilder(_httpClient);
        Invites = new InvitesRequestBuilder(_httpClient);
        OAuth2 = new OAuth2RequestBuilder(_httpClient);
        Packs = new PacksRequestBuilder(_httpClient);
    }

    internal static JsonSerializerOptions DefaultJsonOptions { get; } = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        TypeInfoResolver = JsonTypeInfoResolver.Combine(DtoJsonContext.Default, RestDtoContext.Default)
    };

    public GatewayRequestBuilder Gateway { get; }
    public UsersRequestBuilder Users { get; }
    public GuildsRequestBuilder Guilds { get; }
    public ChannelsRequestBuilder Channels { get; }
    public OAuth2RequestBuilder OAuth2 { get; }
    public InvitesRequestBuilder Invites { get; }
    public PacksRequestBuilder Packs { get; }
    
}