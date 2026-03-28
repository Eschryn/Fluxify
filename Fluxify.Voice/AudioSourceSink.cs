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

using FFMpegCore.Pipes;
using LiveKit.Rtc;

namespace Fluxify.Voice;

public class AudioSourceSink : IPipeSink, IDisposable
{
    private readonly AudioSource _source;
    private readonly int _sendBufferByteSize;
    private readonly short[] _sendBuffer;
    private readonly AudioFrame _frame;
    private readonly SemaphoreSlim _activeInputSemaphore = new(1, 1);
    private readonly RingBuffer _ringBuffer;
    private readonly int _buffer;

    internal AudioSourceSink(AudioSource source, int frameMs = 10, int ringBufferSizeSeconds = 5)
    {
        var framesPerSecond = (int)(1 / (frameMs / 1000f));
        var samplesPerChannel = source.SampleRate / framesPerSecond;
        var sendBufferSize = samplesPerChannel * source.NumChannels;

        _source = source;
        _sendBufferByteSize = sendBufferSize * sizeof(short);
        _sendBuffer = new short[sendBufferSize];
        _frame = new AudioFrame(_sendBuffer, source.SampleRate, source.NumChannels, samplesPerChannel);

        _buffer = _sendBufferByteSize * 50;
        _ringBuffer = new RingBuffer(
            size: sendBufferSize * framesPerSecond * ringBufferSizeSeconds,
            maxOverrun: _buffer
        );
    }
    
    internal void Reset()
    {
        _ringBuffer.Reset();
        _sendBuffer.AsSpan().Clear();
    }

    public async Task ReadAsync(Stream inputStream, CancellationToken cancellationToken)
    {
        await _activeInputSemaphore.WaitAsync(cancellationToken);
        try
        {
            Reset();
            
            while (!cancellationToken.IsCancellationRequested)
            {
                while (_ringBuffer.TryWrite(out var memory))
                {
                    var read = await inputStream.ReadAsync(memory.Value, cancellationToken);
                    if (read <= 0)
                    {
                        break;
                    }

                    _ringBuffer.AdvanceWrite(read);
                }
                
                while (_ringBuffer.BytesAvailable >= _buffer && _ringBuffer.TryReadTo(_sendBuffer, _sendBufferByteSize))
                {
                    await _source.CaptureFrameAsync(_frame, cancellationToken);
                }
            }
                
            while (_ringBuffer.TryReadTo(_sendBuffer, _sendBufferByteSize))
            {
                await _source.CaptureFrameAsync(_frame, cancellationToken);
            }
        }
        finally
        {
            _activeInputSemaphore.Release();
        }
    }

    public string GetFormat() => "s16le";

    public void Dispose() => _activeInputSemaphore.Dispose();
}