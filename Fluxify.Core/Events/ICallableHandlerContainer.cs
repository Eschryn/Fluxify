namespace Fluxify.Core.Events;

public interface ICallableHandlerContainer : IHandlerContainer
{
    /// <summary>
    /// Calls the event handlers
    /// </summary>
    Task CallHandlersAsync();
}