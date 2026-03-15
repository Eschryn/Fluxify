using System.Net.WebSockets;
using Fluxify.Core;
using Fluxify.Gateway.Model;
using Fluxify.Gateway.Model.Data;
using Fluxify.Gateway.WebSockets;
using Microsoft.Extensions.Logging;

namespace Fluxify.Gateway;

public sealed partial class GatewayClient
{
    private readonly ILogger _logger;
    private readonly GatewayConfig _config;
    private readonly WebSocketClient<FluxerProtocol, GatewayPayload> _client;

    private int? _lastSequence;
    public string? SessionId { get; private set; }
    private BotTokenCredentials? _credentials;
    private CancellationTokenSource? _connectionTokenSource;

    public ConnectionState ConnectionState
    {
        get;
        private set
        {
            field = value;
            ConnectionStateChanged?.Invoke(value);
        }
    } = ConnectionState.Connecting;

    public GatewayClient(FluxerConfig? config = null, GatewayConfig? gatewayConfig = null)
        : this(
            gatewayConfig ?? new GatewayConfig(),
            config ??= new FluxerConfig(),
            config.LoggerFactory.CreateLogger<GatewayClient>())
    {
    }

    public GatewayClient(GatewayConfig config, FluxerConfig fluxerConfig, ILogger logger)
    {
        _credentials = null;
        _logger = logger;
        _client = new WebSocketClient<FluxerProtocol, GatewayPayload>(
            new FluxerProtocol(fluxerConfig),
            config.WebSocketClientConfig
        );
        _config = config;

        // TODO: Maybe handle ready and resumed differently
        Ready += async r =>
        {
            SessionId = r.SessionId;
            ConnectionState = ConnectionState.Connected;
            Log.Connected(_logger);
        };
        Resumed += async () =>
        {
            ConnectionState = ConnectionState.Connected;
            Log.Connected(_logger);
        };
        ConnectionStateChanged += state =>
        {
            Log.ConnectionStateChanged(_logger, state);
            
            if (ConnectionState is ConnectionState.Disconnected or ConnectionState.Reconnecting)
            {
                _connectionTokenSource?.Cancel();
            }
        };
    }

    public async Task RunAsync(BotTokenCredentials credentials, CancellationToken cancellationToken = default)
    {
        if (!credentials.Validate())
        {
            return;
        }

        _credentials = credentials;

        await LoopAsync(cancellationToken);
    }

    private async Task LoopAsync(CancellationToken cancellationToken)
    {
        ConnectionState = ConnectionState.Connecting;

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                using var connectionTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                var connectionCancellationToken = connectionTokenSource.Token;
                _connectionTokenSource = connectionTokenSource;

                // TODO: receive gateway uri from api
                await _client.ConnectAsync(new Uri("wss://gateway.fluxer.app/?v=1&encoding=json"),
                    connectionCancellationToken);

                while (ConnectionState != ConnectionState.Disconnected)
                {
                    var payload = await _client.ReceiveAsync(connectionCancellationToken);

                    await HandlePayloadAsync(payload, connectionCancellationToken);
                }
            }
            catch (GatewayCloseException e)
            {
                if (e.CloseStatus
                    is (WebSocketCloseStatus)4004 // AUTHENTICATION_FAILED
                    or (WebSocketCloseStatus)4010 // INVALID_SHARD
                    or (WebSocketCloseStatus)4011 // SHARDING_REQUIRED
                    or (WebSocketCloseStatus)4012) // INVALID_API_VERSION
                {
                    Log.GatewayConnectionClosedPermanently(_logger, e, e.CloseStatus, e.CloseStatusDescription);
                    Environment.Exit((int)e.CloseStatus);
                }

                Log.GatewayConnectionClosed(_logger, e.CloseStatus, e.CloseStatusDescription);
            }
            catch (WebSocketException e)
            {
                Log.WebSocketException(_logger, e);
            }
            catch (Exception e)
            {
                // other exception happened during gateway payload processing - its probably not recoverable
                Log.GatewayPayloadException(_logger, e);
            }
            finally
            {
                _connectionTokenSource = null;   
            }

            StopHeartbeat();
            _client.ResetSocket();
            // TODO: implement backoff
            ConnectionState = ConnectionState.Reconnecting;
        }
    }

    private async Task HandlePayloadAsync(GatewayPayload? payload, CancellationToken cancellationToken)
    {
        switch (payload?.Opcode)
        {
            case GatewayOpCode.Dispatch:
                _lastSequence = payload.Sequence;
                HandleDispatch(payload.Type!, payload.Data!);
                break;
            case GatewayOpCode.Hello:
                InitializeClientHeartbeat(payload.Data!);
                await LoginAsync(cancellationToken);
                break;
            case GatewayOpCode.Reconnect:
                Log.ServerRequestedReconnect(_logger);
                await DisconnectAsync(
                    status: WebSocketCloseStatus.NormalClosure,
                    description: "Requested by server",
                    invalidateSession: false,
                    cancellationToken: cancellationToken);
                break;
            case GatewayOpCode.InvalidSession:
                Log.ServerInvalidatedSession(_logger);
                await DisconnectAsync(
                    status: WebSocketCloseStatus.NormalClosure,
                    description: "Requested by server",
                    invalidateSession: !(payload.Data as bool? ?? false),
                    cancellationToken: cancellationToken);
                break;
            case GatewayOpCode.Heartbeat:
                Log.ServerRequestedHeartbeat(_logger);
                RequestHeartbeat();
                break;
            case GatewayOpCode.HeartbeatAck:
                Log.ServerAcknowledgedHeartbeat(_logger);
                AcknowledgeHeartbeatResponse();
                break;
            case null:
                // this should only happen if the JSON couldn't deserialize.
                // since we don't have any insight, we expect that FluxerProtocol has already logged the exception  
                break;
            default:
                Log.UnexpectedOpCode(_logger, payload.Opcode);
                break;
        }
    }

    public async Task UpdatePresenceAsync(PresenceUpdate data, CancellationToken cancellationToken = default) 
        => await _client.SendAsync(new GatewayPayload(GatewayOpCode.PresenceUpdate, data), cancellationToken);

    //public async Task UpdateVoiceStateAsync(VoiceStateUpdate data, CancellationToken cancellationToken = default)
    //    => await _client.SendAsync(new GatewayPayload(GatewayOpCode.VoiceStateUpdate, data), cancellationToken);
    
    private async Task LoginAsync(CancellationToken cancellationToken)
    {
        ConnectionState = ConnectionState.Authenticating;

        var payload = SessionId switch
        {
            not null => new GatewayPayload(
                GatewayOpCode.Resume,
                new ResumePayloadData(
                    Token: _credentials!.Token, // Should never be null
                    SessionId: SessionId,
                    LastSequence: _lastSequence)),
            null => new GatewayPayload(
                Opcode: GatewayOpCode.Identify,
                new IdentifyPayloadData(
                    Token: _credentials!.Token,
                    Properties: _config.DeviceProperties,
                    _config.IgnoredGatewayEvents,
                    _config.DefaultPresence
                ))
        };

        await _client.SendAsync(payload, cancellationToken);
    }

    private async Task DisconnectAsync(WebSocketCloseStatus status, string description, bool invalidateSession,
        CancellationToken cancellationToken = default)
    {
        if (invalidateSession)
        {
            SessionId = null;
        }

        await _client.DisconnectAsync(status, description, cancellationToken);
        ConnectionState = ConnectionState.Disconnected;
    }
}