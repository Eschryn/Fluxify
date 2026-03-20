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
using Fluxify.Application.Entities.Guilds;
using Fluxify.Core.Types;

namespace Fluxify.Application.Entities.Messages;

public static class MessageExtensions
{
    extension(Message message)
    {
        public Task ReactAsync(IEmoji emoji, CancellationToken cancellationToken = default)
            => message.ReactAsync(
                emoji switch
                {
                    GuildEmoji ge => ge.Id.ToString(CultureInfo.InvariantCulture),
                    UnicodeEmoji ue => ue.Name,
                    _ => throw new ArgumentOutOfRangeException(nameof(emoji), emoji, null)
                },
                cancellationToken
            );

        public Task ReactAsync(Snowflake emojiId, CancellationToken cancellationToken = default)
            => message.ReactAsync(emojiId.ToString(CultureInfo.InvariantCulture), cancellationToken);
    }
}