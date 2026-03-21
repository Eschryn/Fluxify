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

using System.IO.Pipelines;
using System.Net.WebSockets;

namespace Fluxify.Gateway.WebSockets;

/// <summary>
/// 
/// </summary>
/// <param name="config"></param>
internal sealed class WebSocketClient<TProtocol, TFrame>(
    TProtocol protocol,
    WebSocketClientConfig config
)
    where TProtocol : IWebSocketProtocol<TFrame>
    where TFrame : class
{
    private readonly TProtocol _protocol = protocol;
    private readonly Pipe _sendPipe = new();
    private readonly Pipe _receivePipe = new();
    private readonly SemaphoreSlim _sendSemaphore = new(1);
    private ClientWebSocket _clientWebSocket = new();

    private WebSocketClientConfig Config { get; } = config;

    public async Task ConnectAsync(Uri uri, CancellationToken cancellationToken) =>
        await _clientWebSocket.ConnectAsync(uri, cancellationToken);

    public async Task DisconnectAsync(WebSocketCloseStatus status, string description,
        CancellationToken cancellationToken)
    {
        if (_clientWebSocket.State is not (WebSocketState.Aborted or WebSocketState.Closed))
        {
            await _clientWebSocket.CloseAsync(status, description, cancellationToken);
        }
        else
        {
            _clientWebSocket.Dispose();
        }
    }

    public void ResetSocket()
    {
        _clientWebSocket.Dispose();
        
        _sendPipe.Writer.Complete();
        _sendPipe.Reader.Complete();
        _sendPipe.Reset();
        _receivePipe.Writer.Complete();
        _receivePipe.Reader.Complete();
        _receivePipe.Reset();
        
        _clientWebSocket = new ClientWebSocket();
    }

    public async Task SendAsync(TFrame payload, CancellationToken cancellationToken)
    {
        await _sendSemaphore.WaitAsync(cancellationToken);

        try
        {
            var serializeTask = _protocol.SerializeAsync(_sendPipe.Writer, payload, cancellationToken);
            var sendTask = SendPipeContent(WebSocketMessageType.Text, cancellationToken);

            await Task.WhenAll(serializeTask, sendTask);
            
            _sendPipe.Reset();
        }
        finally
        {
            _sendSemaphore.Release();
        }
    }

    public async Task<TFrame?> ReceiveAsync(CancellationToken cancellationToken)
    {
        try
        {
            var receiveTask = FillPipe(cancellationToken);
            var parseTask = _protocol.DeserializeAsync(_receivePipe.Reader, cancellationToken);

            await Task.WhenAll(receiveTask, parseTask);

            return parseTask.Result;
        }
        finally
        {
            _receivePipe.Reset();
        }
    }

    private async Task SendPipeContent(WebSocketMessageType messageType, CancellationToken cancellationToken)
    {
        ReadResult read;
        do
        {
            read = await _sendPipe.Reader.ReadAsync(cancellationToken);
            if (read.Buffer.IsSingleSegment)
            {
                await _clientWebSocket.SendAsync(read.Buffer.First, messageType, read.IsCompleted, cancellationToken);
            }
            else
            {
                foreach (var segment in read.Buffer)
                {
                    await _clientWebSocket.SendAsync(segment, messageType, read.IsCompleted, cancellationToken);
                }
            }

            _sendPipe.Reader.AdvanceTo(read.Buffer.End);
        } while (!(read.IsCompleted || read.IsCanceled));

        await _sendPipe.Reader.CompleteAsync();
    }

    private async Task<WebSocketMessageType> FillPipe(CancellationToken cancellationToken)
    {
        Exception? exception = null;
        ValueWebSocketReceiveResult result;
        try
        {
            do
            {
                var buffer = _receivePipe.Writer.GetMemory(Config.ReceiveBufferSize);
                
                result = await _clientWebSocket.ReceiveAsync(buffer, cancellationToken);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    throw new GatewayCloseException(
                        _clientWebSocket.CloseStatus,
                        _clientWebSocket.CloseStatusDescription);
                }

                _receivePipe.Writer.Advance(result.Count);
                await _receivePipe.Writer.FlushAsync(cancellationToken);
            } while (!result.EndOfMessage || _clientWebSocket.State != WebSocketState.Open);
        }
        catch (Exception e)
        {
            exception = e;
            throw;
        }
        finally
        {
            await _receivePipe.Writer.CompleteAsync(exception);
        }

        return result.MessageType;
    }
}