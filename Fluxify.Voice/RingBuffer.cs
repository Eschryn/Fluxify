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

using System.Diagnostics.CodeAnalysis;

namespace Fluxify.Voice;

internal class RingBuffer(int size, int maxOverrun)
{
    private readonly byte[] _ringBuffer = new byte[size];
    private int _ringBufferReadHead = 0;
    private int _ringBufferWriteHead = 0;
    private int _bytesAvailable;
    
    public int BytesAvailable => _bytesAvailable;

    public bool TryReadTo(Array array, int count)
    {
        if (_bytesAvailable < count)
        {
            return false;
        }

        var start = _ringBufferReadHead;
        _ringBufferReadHead = (_ringBufferReadHead + count) % _ringBuffer.Length;
        if (start > _ringBufferReadHead)
        {
            Buffer.BlockCopy(_ringBuffer, start, array, 0, count - _ringBufferReadHead);
            Buffer.BlockCopy(_ringBuffer, 0, array, 0, _ringBufferReadHead);
        }
        else
        {
            Buffer.BlockCopy(_ringBuffer, start, array, 0, count);
        }
            
        Interlocked.Add(ref _bytesAvailable, -count);
            
        return true;

    }
    
    public bool TryWrite([NotNullWhen(true)] out Memory<byte>? memory)
    {
        if (_bytesAvailable >= maxOverrun)
        {
            memory = null;
            return false;
        }
        
        memory = _ringBuffer.AsMemory()[_ringBufferWriteHead..];
        return true;
    }
    
    public void AdvanceWrite(int count)
    {
        _ringBufferWriteHead = (_ringBufferWriteHead + count) % _ringBuffer.Length;
        Interlocked.Add(ref _bytesAvailable, count);
    }

    public void Reset()
    {
        _ringBufferReadHead = 0;
        _ringBufferWriteHead = 0;
        _bytesAvailable = 0;
        _ringBuffer.AsSpan().Clear();
    }
}