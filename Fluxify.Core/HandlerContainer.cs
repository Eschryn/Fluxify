namespace Fluxify.Core;

public sealed class HandlerContainer : IHandlerContainer
{
    private readonly HashSet<Func<Task>> _handlers = [];
    public void InsertDelegate(Func<Task> handler) => _handlers.Add(handler);
    public void RemoveDelegate(Func<Task> handler) => _handlers.Remove(handler);

    public async Task CallHandlersAsync(object eventPayload)
    {
        var tasks = _handlers.Select(h => h.Invoke());
        await Task.WhenAll(tasks).ConfigureAwait(false);
    }
}