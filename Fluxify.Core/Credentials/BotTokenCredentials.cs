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
using Fluxify.Core.Types;

namespace Fluxify.Core.Credentials;

public class BotTokenCredentials(string token) : ITokenCredentials
{
    private const string TypeConst = "Bot";
    public string Token { get; } = token;

    private Snowflake? _snowflake;
    public Snowflake UserId => _snowflake ??= Snowflake.Parse(
        Token.Split('.', 2)[0],
        CultureInfo.InvariantCulture
    );

    public bool Validate()
    {
        if (Token.Contains('.')
            && ulong.TryParse(Token.Split('.', 2)[0], out var snowflake))
        {
            // cache result
            _snowflake = new Snowflake(snowflake);
        }
        else
        {
            return false;
        }
        
        return true;
    }

    public string GetAuthorizationHeaderValue() => $"{TypeConst} {Token}";
}