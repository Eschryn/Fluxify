using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Fluxify.Core;
using Fluxify.Dto.Json;
using Fluxify.Rest.Channel;
using Fluxify.Rest.Model;
using Microsoft.Extensions.DependencyInjection;

namespace Fluxify.Rest;

public class RestClient(FluxerConfig config)
{
    private readonly FluxerConfig _config = config;
    private readonly HttpClient _httpClient = config.ServiceProvider.GetRequiredService<HttpClient>();

    internal static JsonSerializerOptions DefaultJsonOptions { get; } = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        TypeInfoResolver = JsonTypeInfoResolver.Combine(DtoJsonContext.Default, RestResponsesContext.Default)
    };
    
    public GuildsRequestBuilder Guilds => new(_httpClient);
    public ChannelsRequestBuilder Channels => new(_httpClient);
}