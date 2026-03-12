using System.Collections.Concurrent;
using Fluxify.Application.Entities;
using Fluxify.Core.Types;

namespace Fluxify.Application.State;

public class PermanentCache<TData, TMapper>(TMapper mapper)
    where TData : class, IEntity
    where TMapper : IUpdateEntity<TData>
{
    private readonly ConcurrentDictionary<Snowflake, TData> _dataContainer = new();
    private readonly ResourceLock _lock = new();

    public bool IsCached(Snowflake id) =>  _dataContainer.ContainsKey(id);
    public T? GetCachedOrDefault<T>(Snowflake id) where T : TData => (T?)_dataContainer.GetValueOrDefault(id);
    
    public async Task<TData> GetOrCreateAsync(Snowflake id, Func<Snowflake, Task<TData>> factory, bool bypassCache = false)
    {
        if (bypassCache)
        {
            var entity = await factory(id);
            _dataContainer.TryAdd(id, entity);
            return entity;
        }
        
        if (_dataContainer.TryGetValue(id, out var data))
        {
            return data;
        }

        using var _ = await _lock.LockAsync(id);

        // we need to check again because maybe we got data from another thread
        if (_dataContainer.TryGetValue(id, out data))
        {
            return data;
        }

        return _dataContainer[id] = await factory(id);
    }

    public TData UpdateOrCreate(TData data)
        => _dataContainer.AddOrUpdate(data.Id,
            _ => data,
            (_, existing) => mapper.UpdateEntity(existing, data));
    
    public void Remove(Snowflake id) => _dataContainer.TryRemove(id, out _);
    public void Clear() => _dataContainer.Clear();
}