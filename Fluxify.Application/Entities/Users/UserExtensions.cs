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

using Fluxify.Application.Entities.Channels;
using Fluxify.Application.Entities.Messages;
using Fluxify.Application.Model.Messages;

namespace Fluxify.Application.Entities.Users;

public static class UserExtensions
{
    public static async Task<Message?> SendMessageAsync(this IUser user, string content)
        => await (await user.GetOrCreateDmAsync()).SendMessageAsync(content);
    
    public static async Task<Message?> SendMessageAsync(this IUser user, Action<MessageBuilder> builder)
        => await (await user.GetOrCreateDmAsync()).SendMessageAsync(builder);
    
    public static async Task<Message?> SendMessageAsync(this IUser user, MessageCreate create)
        => await (await user.GetOrCreateDmAsync()).SendMessageAsync(create);
}