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

using Fluxify.Core.Types;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace Fluxify.AspNetCore.Authentication;

public class FluxerChallengeProperties : OAuthChallengeProperties
{
    public const string PermissionKey = "permissions";
    public const string GuildIdKey = "guild_id";
    public const string DisableGuildSelectKey = "disable_guild_select";
    public const string PromptKey = "prompt";
    
    public FluxerChallengeProperties() {}
    
    public FluxerChallengeProperties(
        IDictionary<string, string?> items
    ) : base(items) {}
    
    public FluxerChallengeProperties(
        IDictionary<string, string?> items,
        IDictionary<string, object?>? parameters
    ) : base(items, parameters) {}

    public Permissions? Permissions
    {
        get => GetParameter<Permissions?>("permissions");
        set => SetParameter("permissions", value);
    }

    public Snowflake? GuildId
    {
        get => GetParameter<Snowflake?>("guild_id");
        set => SetParameter("guild_id", value);
    }
    
    public bool? DisableGuildSelect
    {
        get => GetParameter<bool?>("disable_guild_select");
        set => SetParameter("disable_guild_select", value);
    }

    public PromptType? Prompt
    {
        get => GetParameter<PromptType?>("prompt");
        set => SetParameter("prompt", value);
    }
}