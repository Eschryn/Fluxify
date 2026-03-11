using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Fluxify.Core;
using Fluxify.Dto.Json;
using Fluxify.Rest.Channel;
using Microsoft.Extensions.DependencyInjection;

namespace Fluxify.Rest;

public class RestClient(FluxerConfig config)
{
    private readonly FluxerConfig _config = config;
    private readonly HttpClient _httpClient = config.ServiceProvider.GetRequiredService<HttpClient>();

    internal static JsonSerializerOptions DefaultJsonOptions { get; } = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        TypeInfoResolver = JsonTypeInfoResolver.Combine(DtoJsonContext.Default)
    };
    
    public ChannelsRequestBuilder Channels => new(_httpClient);
}