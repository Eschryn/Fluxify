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

using System.Net.WebSockets;
using Fluxify.Gateway.Model;
using Fluxify.Gateway.Model.Data;

namespace Fluxify.Gateway;

public sealed partial class GatewayClient
{
    private long _lastServerHeartbeatEvent;
    private Timer? _heartbeatTimer;
    private TimeSpan? _currentHeartbeatInterval;

    
    private async Task SendHeartbeatAsync(CancellationToken cancellationToken = default)
    {
        await _client.SendAsync(new GatewayPayload(GatewayOpCode.Heartbeat, _lastSequence), cancellationToken);
        Log.HeartbeatSent(_logger, _lastSequence);
    }

    private void ProcessHeartbeatRequest(object? state)
        => Task.Run(async () =>
        {
            if (await ValidateServerHeartbeat())
            {
                await SendHeartbeatAsync();
            }
        });

    private void InitializeClientHeartbeat(object packetData)
    {
        if (packetData is not HelloPayloadData data)
        {
            Log.InvalidHelloEvent(_logger);
            return;
        }

        var heartbeatInterval = TimeSpan.FromMilliseconds(data.HeartbeatInterval);
        var initialHeartbeatJitter = Random.Shared.NextDouble();
        var firstHeartbeatOffset = heartbeatInterval * initialHeartbeatJitter;
        
        Log.HelloReceived(_logger, heartbeatInterval);

        _currentHeartbeatInterval = heartbeatInterval;
        _lastServerHeartbeatEvent = TimeProvider.System.GetTimestamp();
        _heartbeatTimer = new Timer(
            callback: ProcessHeartbeatRequest,
            state: true,
            dueTime: firstHeartbeatOffset,
            period: heartbeatInterval);
    }

    private void StopHeartbeat()
    {
        if (_heartbeatTimer == null)
        {
            return;
        }

        // Disable & delete
        try
        {
            _heartbeatTimer.Change(Timeout.Infinite, Timeout.Infinite);
            _heartbeatTimer.Dispose();
        }
        finally
        {
            _heartbeatTimer = null;
            _currentHeartbeatInterval = null;
        }
    }

    private void AcknowledgeHeartbeatResponse() => _lastServerHeartbeatEvent = TimeProvider.System.GetTimestamp();

    private async Task<bool> ValidateServerHeartbeat()
    {
        if (TimeProvider.System.GetElapsedTime(_lastServerHeartbeatEvent) > _currentHeartbeatInterval * 1.2)
        {
            Log.ServerDidNotRespond(_logger);
            
            await DisconnectAsync(
                status: WebSocketCloseStatus.ProtocolError,
                description: "Server did not respond to heartbeat",
                invalidateSession: false);
            
            return false;
        }
        
        return true;
    }

    private void RequestHeartbeat()
    {
        // only when heartbeat is started the server is allowed to request heartbeats
        var currentHeartbeatInterval = _currentHeartbeatInterval ?? throw new InvalidOperationException();
        
        _lastServerHeartbeatEvent = TimeProvider.System.GetTimestamp();
        
        // start timer immediately
        ThreadPool.QueueUserWorkItem(ProcessHeartbeatRequest, true);
        
        // reset the timer interval and fire next heartbeat earlier in case we are running behind
        // this is an opportunity to catch up with the server!
        var offsetJitter = Random.Shared.NextDouble();
        (_heartbeatTimer ?? throw new InvalidOperationException())
            .Change(currentHeartbeatInterval * 0.7 * offsetJitter, currentHeartbeatInterval);
    }
}