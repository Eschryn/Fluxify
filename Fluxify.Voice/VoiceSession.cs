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

using System.Reactive.Linq;
using System.Reactive.Subjects;
using Fluxify.Application.Entities.Channels.Guilds;
using Fluxify.Core.Types;
using Fluxify.Gateway;
using Fluxify.Gateway.Model.Data.Voice;
using LiveKit.Proto;
using LiveKit.Rtc;
using TrackPublishOptions = LiveKit.Rtc.TrackPublishOptions;

namespace Fluxify.Voice;

public class VoiceSession : IAsyncDisposable
{
    private Snowflake? _channelId;
    private Snowflake? _guildId;
    private string? _connectionId;
    private readonly GatewayClient _gatewayClient;
    private readonly Room _room;
    private readonly AudioSource _audioSource;
    private readonly LocalAudioTrack _localAudioTrack;
    private LocalTrackPublication? _localTrackPublication;
    private readonly BehaviorSubject<UpdateVoiceState> _voiceStateSubject;
    private readonly IDisposable _voiceStateSubscription;
    private TaskCompletionSource? _connectTaskCompletionSource;

    public event EventHandler? OnConnected;

    public event EventHandler<DisconnectReason>? OnDisconnected
    {
        add => _room.Disconnected += value;
        remove => _room.Disconnected -= value;
    }

    public bool Muted
    {
        get => _voiceStateSubject.Value.Mute;
        set => _voiceStateSubject.OnNext(
            new UpdateVoiceState(
                _voiceStateSubject.Value.GuildId,
                _voiceStateSubject.Value.ChannelId,
                value,
                _voiceStateSubject.Value.Deaf
            ));
    }

    public bool Deafened
    {
        get => _voiceStateSubject.Value.Deaf;
        set => _voiceStateSubject.OnNext(
            new UpdateVoiceState(
                _voiceStateSubject.Value.GuildId,
                _voiceStateSubject.Value.ChannelId,
                _voiceStateSubject.Value.Mute,
                value
            ));
    }
    
    public bool IsConnected => _room.IsConnected;
    
    public AudioSourceSink AudioSourceSink { get; }

    public VoiceSession(GatewayClient gatewayClient)
    {
        _gatewayClient = gatewayClient;
        _room = new Room();
        _audioSource = new AudioSource(48000, 2);
        _localAudioTrack = _audioSource.CreateTrack();
        AudioSourceSink = new AudioSourceSink(_audioSource, 15, 7);
        _voiceStateSubject = new BehaviorSubject<UpdateVoiceState>(
            new UpdateVoiceState(0L, 0L, false, false));
        
        _room.Connected += RoomOnConnected;
        gatewayClient.VoiceStateUpdate += OnVoiceStateUpdate;
        gatewayClient.VoiceServerUpdate += OnVoiceServerUpdate;
        _voiceStateSubscription = _voiceStateSubject
            .Throttle(TimeSpan.FromMilliseconds(10))
            .Subscribe(UpdateVoiceState);
    }


    public async Task ConnectAsync(GuildVoiceChannel voiceChannel)
    {
        _connectionId = null;
        _voiceStateSubject.OnNext(
            new UpdateVoiceState(
                (_guildId = voiceChannel.Guild.Id).Value,
                (_channelId = voiceChannel.Id).Value,
                false,
                false
            ));

        try
        {
            _connectTaskCompletionSource = new TaskCompletionSource();
            await _connectTaskCompletionSource.Task;
        }
        finally
        {
            _connectTaskCompletionSource = null;
        }
    }

    public async ValueTask DisconnectAsync()
    {
        if (_room.IsConnected)
        {
            if (_room.LocalParticipant is { } participant && _localTrackPublication is { } publication)
            {
                await participant.UnpublishTrackAsync(publication.Sid);
            }

            await _room.DisconnectAsync();
        }
    }

    private void UpdateVoiceState(UpdateVoiceState data) => _ = _gatewayClient.UpdateVoiceStateAsync(data);

    private void RoomOnConnected(object? sender, EventArgs e)
    {
        _connectTaskCompletionSource?.TrySetResult();

        OnConnected?.Invoke(this, EventArgs.Empty);
    }

    private async Task OnVoiceStateUpdate(VoiceStateResponse arg)
    {
        if (_connectionId != null && arg.ConnectionId != _connectionId
            || _connectionId == null && (arg.ChannelId != _channelId || arg.GuildId != _guildId))
        {
            return;
        }

        _guildId = arg.GuildId;
        _channelId = arg.ChannelId;
        _connectionId = arg.ConnectionId;
    }

    private async Task OnVoiceServerUpdate(GatewayVoiceServer arg)
    {
        if (_connectionId != null && arg.ConnectionId != _connectionId
            || _connectionId == null && (arg.ChannelId != _channelId || arg.GuildId != _guildId))
        {
            return;
        }

        _guildId = arg.GuildId;
        _channelId = arg.ChannelId;
        _connectionId = arg.ConnectionId;

        if (_room.IsConnected)
        {
            if (_localTrackPublication != null)
            {
                await _room.LocalParticipant!.UnpublishTrackAsync(_localTrackPublication.Sid);
            }
            
            _localTrackPublication = null;
            await DisconnectAsync();
        }

        await _room.ConnectAsync(arg.Endpoint, arg.Token);
        _localTrackPublication = await _room.LocalParticipant!.PublishTrackAsync(_localAudioTrack, new TrackPublishOptions()
        {
            Source = TrackSource.SourceMicrophone,
            AudioEncoding = new AudioEncodingOptions()
            {
                MaxBitrate = 510000
            }
        });
    }

    public async ValueTask DisposeAsync()
    {
        await DisconnectAsync();

        _gatewayClient.VoiceServerUpdate -= OnVoiceServerUpdate;
        _gatewayClient.VoiceStateUpdate -= OnVoiceStateUpdate;

        _room.Dispose();
        _audioSource.Dispose();
        _localAudioTrack.Dispose();
        _voiceStateSubscription.Dispose();
        _localAudioTrack.Dispose();
        _room.Dispose();
        _voiceStateSubject.Dispose();
        AudioSourceSink.Dispose();
    }
}