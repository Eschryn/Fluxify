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

using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using Fluxify.Core.Types;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace Fluxify.AspNetCore.Authentication;

public class FluxerHandler(IOptionsMonitor<FluxerOptions> options, ILoggerFactory logger, UrlEncoder encoder)
    : OAuthHandler<FluxerOptions>(options, logger, encoder)
{
    protected override async Task<AuthenticationTicket> CreateTicketAsync(ClaimsIdentity identity, AuthenticationProperties properties, OAuthTokenResponse tokens)
    {
        using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, Options.UserInformationEndpoint);
        httpRequestMessage.Headers.Add("Authorization", "Bearer " + tokens.AccessToken);

        using var httpResponseMessage = await Options.Backchannel.SendAsync(
            httpRequestMessage, 
            HttpCompletionOption.ResponseHeadersRead
        );
        
        using var user = await httpResponseMessage.Content.ReadFromJsonAsync<JsonDocument>() 
                         ?? JsonDocument.Parse("{}");
        
        var context = new OAuthCreatingTicketContext(
            principal: new ClaimsPrincipal(identity),
            properties: properties,
            context: Context,
            scheme: Scheme,
            options: Options,
            backchannel: Backchannel,
            tokens: tokens,
            user: user.RootElement
        );
        
        context.RunClaimActions();
        await Events.CreatingTicket(context);
        
        return new AuthenticationTicket(context.Principal!, context.Properties, Scheme.Name);
    }

    protected override string BuildChallengeUrl(AuthenticationProperties properties, string redirectUri)
    {
        var baseChallengeUrl = base.BuildChallengeUrl(properties, redirectUri);
        var urlBuilder = new UriBuilder(baseChallengeUrl);
        
        var parameters = QueryHelpers.ParseQuery(urlBuilder.Query);
        
        AppendUrlParameter(properties, parameters, "prompt", Options.Prompt);
        AppendUrlParameter<Snowflake?>(properties, parameters, "guild_id");
        AppendUrlParameter(properties, parameters, "permissions", Options.Permissions);
        AppendUrlParameter(properties, parameters, "disable_guild_select", Options.DisableGuildSelect);

        urlBuilder.Query = "";
        return QueryHelpers.AddQueryString(urlBuilder.ToString(), parameters);
    }

    private static void AppendUrlParameter<T>(AuthenticationProperties properties, Dictionary<string, StringValues> parameters, string key, T? fallback = default, Func<T, string?>? format = null)
    {
        format ??= p => p?.ToString()?.ToLowerInvariant();
        
        if (properties.GetParameter<T>(key) is {} param 
            && format(param) is {} str
            && !string.IsNullOrEmpty(str))
        {
            parameters.Add(key, str);
        }
        else if (fallback is {} fallbackObject
                 && format(fallbackObject) is {} fallbackStr
                 && !string.IsNullOrEmpty(fallbackStr))
        {
            parameters.Add(key, fallbackStr);
        }
    }
}