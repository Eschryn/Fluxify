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

using System.Diagnostics.CodeAnalysis;
using Fluxify.Application.Entities.Channels;
using Fluxify.Application.Entities.Guilds;
using Fluxify.Application.Entities.Messages;
using Fluxify.Application.Entities.Users;
using Fluxify.Application.State.Ref;
using Fluxify.Core.Types;

namespace Fluxify.Application.EventArgs;

[method: SetsRequiredMembers]
public class ReactionEventArgs(
    IEmoji emoji,
    ICacheRef<ITextChannel> channel,
    ICacheRef<Message>? updated,
    ICacheRef<Guild>? guild,
    ICacheRef<IUser>? user
) {
    public required IEmoji Emoji { get; init; } = emoji;
    public required ICacheRef<ITextChannel> Channel { get; init; } = channel;
    public ICacheRef<Message>? Message { get; init; } = updated;
    public ICacheRef<Guild>? Guild { get; init; } = guild;
    public ICacheRef<IUser>? User { get; init; } = user;
}