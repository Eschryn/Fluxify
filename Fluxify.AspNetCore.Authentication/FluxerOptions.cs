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

using System.Security.Claims;
using Fluxify.Core.Types;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;

namespace Fluxify.AspNetCore.Authentication;

public class FluxerOptions : OAuthOptions
{

    public Uri InstanceUrl
    {
        get;
        set
        {
            field = value;
            UpdateEndpoints();
        }
    }

    public bool? DisableGuildSelect { get; set; }
    public Permissions? Permissions { get; set; }
    public PromptType? Prompt { get; set; }
    

    public FluxerOptions()
    {
        CallbackPath = new PathString("/signin-fluxer");
        InstanceUrl = new("https://api.fluxer.app/");
        
        Scope.Add("identify");

        // https://github.com/fluxerapp/fluxer/blob/d91388b979e7709575f929218dd053e081aa684e/packages/api/src/user/UserMappers.tsx#L225
        ClaimActions.MapJsonKey(ClaimTypes.Name, "username");
        ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
        ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
        ClaimActions.MapJsonKey(FluxerClaimTypes.Discriminator, "discriminator");
        ClaimActions.MapJsonKey(FluxerClaimTypes.GlobalName, "permissions");
        ClaimActions.MapJsonKey(FluxerClaimTypes.AvatarHash, "guild_id");
        ClaimActions.MapJsonKey(FluxerClaimTypes.Flags, "disable_guild_select");
    }

    private void UpdateEndpoints()
    {
        TokenEndpoint = FluxerAuthenticationDefaults.GetUri(InstanceUrl, FluxerAuthenticationDefaults.TokenPath).ToString();
        AuthorizationEndpoint = FluxerAuthenticationDefaults.GetUri(InstanceUrl, FluxerAuthenticationDefaults.AuthorizationPath).ToString();
        UserInformationEndpoint = FluxerAuthenticationDefaults.GetUri(InstanceUrl, FluxerAuthenticationDefaults.UserInformationPath).ToString();
    }
}