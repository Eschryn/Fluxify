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

namespace Fluxify.Commands.CommandCollection;

public static class CommandCollectionExtensions
{
    extension(ICommandCollection collection)
    {
        public ICommandCollection Module(string name, Action<ICommandCollection> func, string[]? preconditions)
        {
            return collection.Module(new ModuleMeta(name, string.Empty, string.Empty), func, preconditions);
        }

        public ICommandCollection Module(string name, Action<ICommandCollection> func, params Precondition[]? preconditions)
        {
            return collection
                .AddPreconditions(preconditions)
                .Module(new ModuleMeta(name, string.Empty, string.Empty), func, preconditions?.Select(p => p.Name)?.ToArray());
        }

        public ICommandCollection Command(CommandMeta meta, Delegate handler, string[]? preconditions = null)
            => collection.Command(meta, CommandDelegateFactory.Create(handler), preconditions);

        public ICommandCollection Command(string name, Delegate handler, string[]? preconditions)
            => collection.Command(new CommandMeta(name, string.Empty, string.Empty), handler, preconditions);
        public ICommandCollection Command(string name, Delegate handler, params Precondition[]? preconditions)
        {
            return collection
                .AddPreconditions(preconditions)
                .Command(new CommandMeta(name, string.Empty, string.Empty), handler,
                preconditions?.Select(p => p.Name)?.ToArray());
        }

        private ICommandCollection AddPreconditions(Precondition[]? preconditions)
        {
            if (preconditions == null)
            {
                return collection;
            }

            foreach (var precondition in preconditions)
            {
                collection.Precondition(precondition);
            }
            
            return collection;
        }
    }
}