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
using Fluxify.Gateway.WebSockets;
using Microsoft.Extensions.Logging;

namespace Fluxify.Gateway;

public sealed partial class GatewayClient
{
    private static partial class Log
    {
        [LoggerMessage(0, LogLevel.Information, "Received unexpected opcode {opcode}")]
        public static partial void UnexpectedOpCode(ILogger logger, GatewayOpCode opcode);

        [LoggerMessage(2, LogLevel.Error, "Unable to dispatch gateway event. (Is the client out of date?)")]
        public static partial void DispatchException(ILogger logger, Exception ex);

        [LoggerMessage(4, LogLevel.Warning, "Unable to process gateway hello event!")]
        public static partial void InvalidHelloEvent(ILogger logger);

        [LoggerMessage(6, LogLevel.Debug, "Event {packetType} received.")]
        public static partial void EventReceived(ILogger logger, string packetType);

        [LoggerMessage(5, LogLevel.Warning, "Gateway event {packetType} was not handled!")]
        public static partial void UnhandledEvent(ILogger logger, string packetType);

        [LoggerMessage(7, LogLevel.Trace, "Gateway connection state changed: {state}.")]
        public static partial void ConnectionStateChanged(ILogger logger, ConnectionState state);

        [LoggerMessage(8, LogLevel.Error, "Gateway connection closed with [{eCloseStatus}]: {eCloseStatusDescription}")]
        public static partial void GatewayConnectionClosed(ILogger logger, WebSocketCloseStatus? eCloseStatus, string? eCloseStatusDescription);

        [LoggerMessage(9, LogLevel.Trace, "Heartbeat {lastSequence} sent!")]
        public static partial void HeartbeatSent(ILogger logger, int? lastSequence);

        [LoggerMessage(3, LogLevel.Information, "Connected to gateway!")]
        public static partial void Connected(ILogger logger);

        [LoggerMessage(1, LogLevel.Error, "Caught exception from user code:")]
        public static partial void UserCodeException(ILogger logger, Exception exception);
        
        [LoggerMessage(18, LogLevel.Error, "Exception in core gateway payload processing code:")]
        public static partial void GatewayPayloadException(ILogger logger, Exception exception);
        
        [LoggerMessage(10, LogLevel.Error, "Caught exception from user code:")]
        public static partial void WebSocketException(ILogger logger, WebSocketException exception);
        
        [LoggerMessage(11, LogLevel.Error, "Fatal gateway close! Check configuration.\n\tClose Status: {eCloseStatus}\n\tClose Description: {eCloseStatusDescription}")]
        public static partial void GatewayConnectionClosedPermanently(ILogger logger, GatewayCloseException exception, WebSocketCloseStatus? eCloseStatus, string? eCloseStatusDescription);

        [LoggerMessage(12, LogLevel.Trace, "Server requested reconnect.")]
        public static partial void ServerRequestedReconnect(ILogger logger);
        
        [LoggerMessage(13, LogLevel.Trace, "Server invalidated session.")]
        public static partial void ServerInvalidatedSession(ILogger logger);
        
        [LoggerMessage(14, LogLevel.Trace, "Server requested heartbeat.")]
        public static partial void ServerRequestedHeartbeat(ILogger logger);

        [LoggerMessage(15, LogLevel.Trace, "Server acknowledged heartbeat.")]
        public static partial void ServerAcknowledgedHeartbeat(ILogger logger);

        [LoggerMessage(16, LogLevel.Trace, "Received hello from server. Server requested {heartbeat:c} heartbeat interval.")]
        public static partial void HelloReceived(ILogger logger, TimeSpan heartbeat);
        
        [LoggerMessage(17, LogLevel.Warning, "Server did not respond to heartbeat.")]
        public static partial void ServerDidNotRespond(ILogger logger);
        
        [LoggerMessage(19, LogLevel.Warning, "The connection was cancelled. This was probably due to a disconnect")]
        public static partial void ConnectionCancelled(ILogger logger);

        [LoggerMessage(20, LogLevel.Information, "Server acknowledged heartbeat after {latency:ss's 'fff'ms'}. Average latency: {averageLatency:ss's 'fff'ms'}")]
        public static partial void HeartbeatAcknowledged(ILogger logger, TimeSpan latency, TimeSpan averageLatency);
    }
}