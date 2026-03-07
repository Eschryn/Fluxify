namespace Fluxify.Core;

public sealed class HandlerContainer<T> : IHandlerContainer
{
    private readonly HashSet<Func<T, Task>> _handlers = [];
    public void InsertDelegate(Func<T, Task> handler) => _handlers.Add(handler);
    public void RemoveDelegate(Func<T, Task> handler) => _handlers.Remove(handler);

    public Task CallHandlersAsync(object eventPayload)
    {
        var payload = (T)eventPayload;
        var tasks = _handlers.Select(h =>  h.Invoke(payload));
        return Task.WhenAll(tasks);
    }
}