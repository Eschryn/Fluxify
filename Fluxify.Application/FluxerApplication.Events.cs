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

using Fluxify.Application.Entities.Guilds;
using Fluxify.Application.Entities.Messages;
using Fluxify.Core.Events;

namespace Fluxify.Application;

public partial class FluxerApplication
{
    private readonly HandlerContainer<Message> _messageHandlers = new();
    private readonly HandlerContainer<Guild> _guildCreatedHandlers = new();
    private readonly HandlerContainer<Guild> _guildUpdatedHandlers = new();
    private readonly HandlerContainer<Guild> _guildDeletedHandlers = new();
    
    public event Func<Message, Task> MessageReceived
    {
        add => _messageHandlers.InsertDelegate(value);
        remove => _messageHandlers.RemoveDelegate(value);
    }
    
    public event Func<Guild, Task> GuildCreated
    {
        add => _guildCreatedHandlers.InsertDelegate(value);
        remove => _guildCreatedHandlers.RemoveDelegate(value);
    }
    
    public event Func<Guild, Task> GuildUpdated
    {
        add => _guildUpdatedHandlers.InsertDelegate(value);
        remove => _guildUpdatedHandlers.RemoveDelegate(value);
    }
    
    public event Func<Guild, Task> GuildDeleted
    {
        add => _guildDeletedHandlers.InsertDelegate(value);
        remove => _guildDeletedHandlers.RemoveDelegate(value);
    }


}