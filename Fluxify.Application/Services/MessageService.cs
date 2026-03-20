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

using Fluxify.Application.Entities.Messages;
using Fluxify.Application.Model.Messages;
using Fluxify.Core.Types;
using Fluxify.Rest;

namespace Fluxify.Application.Services;

internal class MessageService(
    RestClient client,
    MessageMapper mapper)
{
    public async Task<Message?> SendMessageAsync(Snowflake channelId, MessageDto messageDto, CancellationToken cancellationToken = default)
    {
        var result = await client.Channels[channelId].Messages
            .SendMessageAsync(mapper.Map(messageDto), cancellationToken);
        
        return result != null ? await mapper.MapAsync(result) : null;
    }
}