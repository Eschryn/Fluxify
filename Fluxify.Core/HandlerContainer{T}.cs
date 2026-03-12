namespace Fluxify.Core;

public sealed class HandlerContainer<T> : IHandlerContainer
{
    private readonly HashSet<Func<T, Task>> _handlers = [];
    public void InsertDelegate(Func<T, Task> handler) => _handlers.Add(handler);
    public void RemoveDelegate(Func<T, Task> handler) => _handlers.Remove(handler);

    public async Task CallHandlersAsync(object eventPayload) => await CallHandlersAsync((T)eventPayload);
    public async Task CallHandlersAsync(T payload)
    {
        var tasks = _handlers.Select(h => h.Invoke(payload)); 
        await Task.WhenAll(tasks).ConfigureAwait(false);
    }
}