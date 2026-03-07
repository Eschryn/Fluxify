using System.IO.Pipelines;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Fluxify.Core;
using Fluxify.Dto.Json;
using Fluxify.Gateway.Model;

namespace Fluxify.Gateway.WebSockets;

public sealed class FluxerProtocol(FluxerConfig fluxerConfig) : IWebSocketProtocol<GatewayPayload>
{
    private JsonSerializerOptions SerializerOptions { get; } = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        TypeInfoResolver = JsonTypeInfoResolver.Combine(
            Model.GatewayJsonContext.Default,
            DtoJsonContext.Default
        )
    };
    
    public async Task<GatewayPayload?> DeserializeAsync(PipeReader pipeReader, CancellationToken cancellationToken = default)
    {
        try
        {
            return await JsonSerializer.DeserializeAsync<GatewayPayload>(pipeReader, SerializerOptions, cancellationToken);
        }
        finally
        {
            await pipeReader.CompleteAsync();
        }
    }

    public async Task SerializeAsync(PipeWriter pipeWriter, GatewayPayload frame, CancellationToken cancellationToken = default)
    {
        await JsonSerializer.SerializeAsync(pipeWriter, frame, SerializerOptions, cancellationToken);
        await pipeWriter.FlushAsync(cancellationToken);
        await pipeWriter.CompleteAsync();
    }
}