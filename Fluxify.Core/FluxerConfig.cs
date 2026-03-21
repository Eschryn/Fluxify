// Copyright 2026 Fluxify Contributors
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Globalization;
using System.Text;
using Fluxify.Core.Credentials;
using Fluxify.Core.DependencyInjection;
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
    public ILoggerFactory LoggerFactory { get; set; }
    public IServiceProvider ServiceProvider { get; set; }
    public Func<FluxerConfig, HttpClient> HttpClientFactory { get; set; } = DefaultHttpClientFactory;

    private static HttpClient DefaultHttpClientFactory(FluxerConfig cfg)
    {
        var socketsHttpHandler = new SocketsHttpHandler()
        {
            UseCookies = false,
            AllowAutoRedirect = false,
            PooledConnectionLifetime = TimeSpan.FromMinutes(15)
        };
        
        
        var authenticationHeaderHandler = cfg.ServiceProvider.GetRequiredService<AuthenticationHeaderHandler>();
        authenticationHeaderHandler.InnerHandler = socketsHttpHandler;
            
        return new HttpClient(authenticationHeaderHandler)
        {
            BaseAddress = cfg.GetApiBaseUri(),
            DefaultRequestVersion = Version.Parse("2.0"),
        };
    }

    public Func<Task<ITokenCredentials>> CredentialProvider { get; set; } 
        = () => Task.FromException<ITokenCredentials>(new InvalidOperationException("No credentials provider set."));

    // todo: move to bot config and mark obsolete
    public ITokenCredentials? Credentials
    {
        get;
        set
        {
            field = value;
            
            if (value is not null)
            {
                CredentialProvider = () => Task.FromResult(value);
            }
        }
    }

    public Uri GetApiBaseUri() => 
        new(InstanceUri, string.Format(CultureInfo.InvariantCulture, VersionPathFormat, ApiVersion));
}