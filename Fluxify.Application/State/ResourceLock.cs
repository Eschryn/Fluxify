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

using System.Collections.Concurrent;
using Fluxify.Core.Types;
using Microsoft.Extensions.ObjectPool;

namespace Fluxify.Application.State;

public class ResourceLock
{
    private readonly ObjectPool<SemaphoreSlim> _semaphorePool =
        new DefaultObjectPoolProvider().Create(new SemaphorePool());

    private readonly ConcurrentDictionary<Snowflake, LockDisposable> _activeLocks = new();

    public async Task<IDisposable> LockAsync(Snowflake id, CancellationToken cancellationToken = default)
    {
        var lockDisposable = _activeLocks.GetOrAdd(id, _ => new LockDisposable(_semaphorePool));

        await lockDisposable.LockAsync(cancellationToken);

        return new DisposeOnce(lockDisposable);
    }

    private class LockDisposable : IDisposable
    {
        private ObjectPool<SemaphoreSlim> _semaphorePool;
        private int _refCount = 0;
        private SemaphoreSlim _semaphore;

        public LockDisposable(ObjectPool<SemaphoreSlim> semaphorePool)
        {
            _semaphorePool = semaphorePool;
            Interlocked.Increment(ref _refCount);
            _semaphore = semaphorePool.Get();
        }

        public Task LockAsync(CancellationToken cancellationToken) => _semaphore.WaitAsync(cancellationToken);

        public void Dispose()
        {
            _semaphore.Release();
            _semaphorePool.Return(_semaphore);
            Interlocked.Decrement(ref _refCount);

            if (_refCount <= 0)
            {
                _semaphorePool = null!;
                _semaphore = null!;
            }
        }
    }

    private class DisposeOnce(IDisposable disposable) : IDisposable
    {
        private IDisposable _disposable = disposable;

        public void Dispose()
        {
            if (_disposable is null) throw new ObjectDisposedException(nameof(DisposeOnce));

            _disposable.Dispose();
            _disposable = null!;
        }
    }
}