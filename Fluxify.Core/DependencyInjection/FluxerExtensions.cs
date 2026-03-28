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

using Fluxify.Core;
using Fluxify.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
// ReSharper disable once CheckNamespace

namespace Microsoft.Extensions.DependencyInjection;

public static class FluxerExtensions
{
    public static IServiceCollection AddFluxifyCore(this IServiceCollection services, Func<IServiceProvider, FluxerConfig> configure)
    {
        services.TryAddSingleton<FluxerConfig>(sp =>
        {
            var fluxerConfig = configure(sp);
            if (fluxerConfig.LoggerFactory == NullLoggerFactory.Instance)
            {
                fluxerConfig.LoggerFactory = sp.GetRequiredService<ILoggerFactory>();
            }

            if (fluxerConfig.ServiceProvider is BasicProvider)
            {
                fluxerConfig.ServiceProvider = sp;
            }

            return fluxerConfig;
        });
        
        services.TryAddTransient<AuthenticationHeaderHandler>();
        services.TryAddTransient(sp =>
        {
            var config = sp.GetRequiredService<FluxerConfig>();
            
            return config.HttpClientFactory.Invoke(config);
        });

        return services;
    }
}