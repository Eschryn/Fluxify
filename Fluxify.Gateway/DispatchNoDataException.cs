namespace Fluxify.Gateway;

public sealed class DispatchNoDataException(string eventType) : DispatchException;