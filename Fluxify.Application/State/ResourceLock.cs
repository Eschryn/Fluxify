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