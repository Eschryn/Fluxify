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

using Fluxify.Application.EventArgs;
using Fluxify.Core.Events;

namespace Fluxify.Application;

public partial class FluxerApplication
{
    private readonly HandlerContainer<MessageEventArgs> _messageCreateHandlers = new();
    private readonly HandlerContainer<MessageEventArgs> _messageUpdateHandlers = new();
    private readonly HandlerContainer<MessageDeletedEventArgs> _messageDeleteHandlers = new();
    private readonly HandlerContainer<MessagesBulkDeletedEventArgs> _messageBulkDeletedHandlers = new();
    private readonly HandlerContainer<GuildEventArgs> _guildCreatedHandlers = new();
    private readonly HandlerContainer<GuildEventArgs> _guildUpdatedHandlers = new();
    private readonly HandlerContainer<GuildDeletedEventArgs> _guildDeletedHandlers = new();
    
    public event Func<MessageEventArgs, Task> MessageReceived
    {
        add => _messageCreateHandlers.InsertDelegate(value);
        remove => _messageCreateHandlers.RemoveDelegate(value);
    }
    
    public event Func<MessageEventArgs, Task> MessageUpdated
    {
        add => _messageUpdateHandlers.InsertDelegate(value);
        remove => _messageUpdateHandlers.RemoveDelegate(value);
    }
    
    public event Func<MessageDeletedEventArgs, Task> MessageDeleted
    {
        add => _messageDeleteHandlers.InsertDelegate(value);
        remove => _messageDeleteHandlers.RemoveDelegate(value);
    }
    
    public event Func<MessagesBulkDeletedEventArgs, Task> MessageBulkDeleted
    {
        add => _messageBulkDeletedHandlers.InsertDelegate(value);
        remove => _messageBulkDeletedHandlers.RemoveDelegate(value);
    }
    
    public event Func<GuildEventArgs, Task> GuildCreated
    {
        add => _guildCreatedHandlers.InsertDelegate(value);
        remove => _guildCreatedHandlers.RemoveDelegate(value);
    }
    
    public event Func<GuildEventArgs, Task> GuildUpdated
    {
        add => _guildUpdatedHandlers.InsertDelegate(value);
        remove => _guildUpdatedHandlers.RemoveDelegate(value);
    }
    
    public event Func<GuildDeletedEventArgs, Task> GuildDeleted
    {
        add => _guildDeletedHandlers.InsertDelegate(value);
        remove => _guildDeletedHandlers.RemoveDelegate(value);
    }


}