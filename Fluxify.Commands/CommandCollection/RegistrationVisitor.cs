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

using System.Collections.Frozen;
using Fluxify.Commands.Model;

namespace Fluxify.Commands.CommandCollection;

internal class RegistrationVisitor(Precondition[] preconditions)
{
    private readonly Dictionary<string, Precondition> _preconditions = preconditions.ToDictionary(p => p.Name);
    public CommandTreeNode Visit(RegistrationEntry entry)
    {
        return entry switch
        {
            CommandRegistration reg => new CommandTreeNode(
                Empty, 
                reg.Handler,
                reg.Meta,
                reg.Preconditions.Select(p => _preconditions[p]).ToArray()),
            ModuleRegistration reg => new CommandTreeNode(
                reg.Children.ToFrozenDictionary(k => k.MetaName, Visit),
                null,
                reg.Meta,
                reg.Preconditions.Select(p => _preconditions[p]).ToArray()),
            _ => throw new InvalidOperationException("Unknown registration entry type")
        };
    }

    
    private static readonly FrozenDictionary<string, CommandTreeNode> Empty 
#if NET10_0_OR_GREATER
        = FrozenDictionary.Create<string, CommandTreeNode>();
#else
        = new Dictionary<string, CommandTreeNode>().ToFrozenDictionary();
#endif
}