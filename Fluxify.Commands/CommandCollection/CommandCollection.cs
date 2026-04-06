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

using Fluxify.Commands.Model;
using Fluxify.Commands.TextCommand;

namespace Fluxify.Commands.CommandCollection;

public class CommandCollection : ICommandCollection
{
    private List<RegistrationEntry> RegistrationEntries { get; set; } = [];
    private Dictionary<string, Precondition> Preconditions { get; set; } = [];

    public ICommandCollection Precondition(Precondition precondition)
    {
        Preconditions.TryAdd(precondition.Name, precondition);
        return this;
    }
    
    public ICommandCollection Module(ModuleMeta meta, Action<ICommandCollection> configure, string[]? preconditions)
    {
        var container = new CommandCollection();
        configure(container);
        RegistrationEntries.Add(new ModuleRegistration(meta, container.RegistrationEntries.ToArray(), preconditions));
        return this;
    }

    public ICommandCollection Command(CommandMeta meta, CommandDelegate handler, string[]? preconditions)
    {
        RegistrationEntries.Add(new CommandRegistration(meta, handler, preconditions));
        return this;
    }

    public TextCommandDispatcher BuildDispatcher(string prefix, IServiceProvider serviceProvider)
        => BuildDispatcher(new CommandConfig(prefix, serviceProvider));
    
    public TextCommandDispatcher BuildDispatcher(CommandConfig config)
    {
        return new TextCommandDispatcher(
            config,
            CommandTreeNode.FromEntries(RegistrationEntries, Preconditions.Values.ToArray())
        );
    }
}