using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fluxify.Core;

public class BasicProvider(FluxerConfig fluxerConfig) : IServiceProvider
{
    public object? GetService(Type serviceType)
    {
        if (serviceType == typeof(IServiceScopeFactory))
        {
            return new ScopeFactory(this);
        }
        else if (serviceType == typeof(HttpClient))
        {
            return fluxerConfig.HttpClientFactory?.Invoke(fluxerConfig);
        }
        else if (serviceType == typeof(ILoggerFactory))
        {
            return fluxerConfig.LoggerFactory;
        }

        return null;
    }

    private class ScopeFactory(IServiceProvider provider) : IServiceScopeFactory
    {
        public IServiceScope CreateScope() => new ServiceScope(provider);

        private class ServiceScope(IServiceProvider provider) : IServiceScope
        {
            public void Dispose()
            {
            }

            public IServiceProvider ServiceProvider { get; } = provider;
        }
    }
}