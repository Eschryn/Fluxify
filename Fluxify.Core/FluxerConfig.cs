using System.Globalization;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Fluxify.Core;

internal class DummyProvider : IServiceProvider
{
    public object? GetService(Type serviceType)
    {
        return null;
    }
}


public class FluxerConfig(ILoggerFactory? loggerFactory = null, IServiceProvider? serviceProvider = null)
{
    private const int ApiVersion  = 1;
    private const int GatewayVersion = 1;
    private static readonly CompositeFormat VersionPathFormat = CompositeFormat.Parse("v{0}/");
    public Uri InstanceUri { get; init; } = new("https://api.fluxer.app/");
    public ILoggerFactory LoggerFactory { get; init; } = loggerFactory ?? serviceProvider?.GetService<ILoggerFactory>() ?? new NullLoggerFactory();
    public IServiceProvider ServiceProvider { get; init; } = serviceProvider ?? new DummyProvider();

    public Uri GetApiBaseUri() => 
        new(InstanceUri, string.Format(CultureInfo.InvariantCulture, VersionPathFormat, ApiVersion));
}