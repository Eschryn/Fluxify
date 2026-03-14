using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Fluxify.Core;
using Fluxify.Dto.Json;
using Fluxify.Rest.Channel;
using Fluxify.Rest.Model;
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

        Users = new UsersRequestBuilder(_httpClient);
        Channels = new ChannelsRequestBuilder(_httpClient);
    }

    internal static JsonSerializerOptions DefaultJsonOptions { get; } = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        TypeInfoResolver = JsonTypeInfoResolver.Combine(DtoJsonContext.Default, RestDtoContext.Default)
    };
    
    public UsersRequestBuilder Users { get; }
    public ChannelsRequestBuilder Channels { get; }
}