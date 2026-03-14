using System.Globalization;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Fluxify.Core;

public class FluxerConfig
{
    public FluxerConfig(ILoggerFactory? loggerFactory = null, IServiceProvider? serviceProvider = null)
    {
        LoggerFactory = loggerFactory ?? serviceProvider?.GetService<ILoggerFactory>() ?? new NullLoggerFactory();
        ServiceProvider = serviceProvider ?? new BasicProvider(this);
    }

    private const int ApiVersion  = 1;
    private const int GatewayVersion = 1;
    private static readonly CompositeFormat VersionPathFormat = CompositeFormat.Parse("v{0}/");
    public Uri InstanceUri { get; init; } = new("https://api.fluxer.app/");
    public ILoggerFactory LoggerFactory { get; init; }
    public IServiceProvider ServiceProvider { get; set; }
    public Func<FluxerConfig, HttpClient>? HttpClientFactory { get; set; } = DefaultHttpClientFactory;

    private static HttpClient DefaultHttpClientFactory(FluxerConfig cfg)
    {
        var socketsHttpHandler = new SocketsHttpHandler()
        {
            UseCookies = false,
            AllowAutoRedirect = false,
            PooledConnectionLifetime = TimeSpan.FromMinutes(15)
        };

        return new HttpClient(socketsHttpHandler)
        {
            BaseAddress = cfg.GetApiBaseUri(),
            DefaultRequestHeaders =
            {
                { "Authorization", cfg.Credentials.GetAuthorizationHeaderValue() }
            }
        };
    }

    public BotTokenCredentials Credentials { get; set; }

    public Uri GetApiBaseUri() => 
        new(InstanceUri, string.Format(CultureInfo.InvariantCulture, VersionPathFormat, ApiVersion));
}