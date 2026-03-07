using System.IO.Pipelines;

namespace Fluxify.Gateway.WebSockets;

public interface IWebSocketProtocol<TFrame>
{
    public Task<TFrame?> DeserializeAsync(PipeReader pipeReader, CancellationToken cancellationToken = default);
    public Task SerializeAsync(PipeWriter pipeWriter, TFrame frame, CancellationToken cancellationToken = default);
}