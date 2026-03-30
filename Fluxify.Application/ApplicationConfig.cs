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

using Fluxify.Application.Common;
using Fluxify.Core;
using Fluxify.Core.Credentials;
using Fluxify.Gateway;

namespace Fluxify.Application;

public class ApplicationConfig
{
    public CacheConfig CacheConfig { get; set; } = new();
    public FluxerConfig FluxerConfig { get; set; } = new();
    public GatewayConfig GatewayConfig { get; set; } = new();
    
    public ITokenCredentials? Credentials
    {
        get;
        set
        {
            field = value;
            
            if (value is not null)
            {
                FluxerConfig.CredentialProvider = () => Task.FromResult(value);
            }
        }
    }
}