using Microsoft.Extensions.ObjectPool;

namespace Fluxify.Application.State;

public class SemaphorePool : IPooledObjectPolicy<SemaphoreSlim>
{
    public SemaphoreSlim Create() => new(1);

    public bool Return(SemaphoreSlim obj)
    {
        if (obj.CurrentCount != 1)
        {
            throw new InvalidOperationException("Semaphore must be in released state");
        }

        return true;
    }
}