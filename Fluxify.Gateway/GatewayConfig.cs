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

using Fluxify.Gateway.Model.Data;
using Fluxify.Gateway.WebSockets;

namespace Fluxify.Gateway;

public class GatewayConfig
{
    public WebSocketClientConfig WebSocketClientConfig { get; set; } = new();
    public TimeSpan SendTimeout { get; set; } = TimeSpan.FromMinutes(30);

    public string[] IgnoredGatewayEvents { get; set; } = [];
    public PresenceUpdate DefaultPresence { get; set; } = new(UserStatus.Online);
    
    // decide if we maybe want to restore a session from a file
    // public string SessionFile { get; set; } = "last_session";
    
    internal Dictionary<string, string> DeviceProperties { get; } = new()
    {
        ["os"] = Environment.OSVersion.Platform.ToString(),
        ["browser"] = "Fluxify.Bot",
        ["device"] = "Fluxify.Bot"
    };
}