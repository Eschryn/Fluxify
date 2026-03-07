using Microsoft.Extensions.DependencyInjection;

namespace Fluxify.Core;

public class DummyProvider : IServiceProvider
{
    public object? GetService(Type serviceType)
    {
        if (serviceType == typeof(IServiceScopeFactory))
        {
            return new ScopeFactory(this);
        }
        return null;
    }
    
    private class ScopeFactory(IServiceProvider provider) : IServiceScopeFactory
    {
        public IServiceScope CreateScope() => new ServiceScope(provider);

        private class ServiceScope(IServiceProvider provider) : IServiceScope
        {
            public void Dispose() { }
            public IServiceProvider ServiceProvider { get; } = provider;
        }
    }
}