using System.IO.Pipelines;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Fluxify.Core;
using Fluxify.Dto.Json;
using Fluxify.Gateway.Model;
using Microsoft.Extensions.Logging;

namespace Fluxify.Gateway.WebSockets;

public sealed partial class FluxerProtocol(FluxerConfig fluxerConfig) : IWebSocketProtocol<GatewayPayload>
{
    private readonly ILogger _logger = fluxerConfig.LoggerFactory.CreateLogger<FluxerProtocol>();
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        TypeInfoResolver = JsonTypeInfoResolver.Combine(
            Model.GatewayJsonContext.Default,
            DtoJsonContext.Default
        ),
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };
    
    public async Task<GatewayPayload?> DeserializeAsync(PipeReader pipeReader, CancellationToken cancellationToken = default)
    {
        try
        {
            return await JsonSerializer.DeserializeAsync<GatewayPayload>(pipeReader, _serializerOptions,
                cancellationToken);
        }
        catch (JsonException e)
        {
            Log.LogDeserializeError(_logger, e);
            return null;
        }
        finally
        {
            await pipeReader.CompleteAsync();
        }
    }

    public async Task SerializeAsync(PipeWriter pipeWriter, GatewayPayload frame, CancellationToken cancellationToken = default)
    {
        await JsonSerializer.SerializeAsync(pipeWriter, frame, _serializerOptions, cancellationToken);
        await pipeWriter.FlushAsync(cancellationToken);
        await pipeWriter.CompleteAsync();
    }

    private static partial class Log
    {
        [LoggerMessage(0, LogLevel.Error, "Error while deserializing payload:")]
        public static partial void LogDeserializeError(ILogger logger, Exception e);
    }
}