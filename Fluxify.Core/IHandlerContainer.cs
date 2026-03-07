namespace Fluxify.Core;

public interface IHandlerContainer
{
    Task CallHandlersAsync(object eventPayload);
}