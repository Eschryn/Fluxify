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
using Fluxify.Commands.CommandCollection;

namespace Fluxify.Commands.Model;

internal record CommandTreeNode(
    FrozenDictionary<string, CommandTreeNode> Commands,
    CommandDelegate? Execute,
    CommandTreeNode? DefaultCommand,
    Meta Meta,
    Precondition[] Preconditions
)
{
    public CommandTreeNode(
        FrozenDictionary<string, CommandTreeNode> commands,
        ModuleMeta meta,
        Precondition[] preconditions
    ) : this(
        commands,
        Execute: null,
        meta.DefaultCommand != null ? commands[meta.DefaultCommand] : null,
        meta,
        preconditions
    ) { }

    public CommandTreeNode(
        FrozenDictionary<string, CommandTreeNode> commands,
        CommandDelegate? execute,
        CommandMeta meta,
        Precondition[] preconditions
    ) : this(
        commands,
        execute,
        null,
        meta,
        preconditions
    ) { }

    private static bool GetDefaultCommand(FrozenDictionary<string, CommandTreeNode> commands, Meta meta,
        out CommandDelegate? defaultCommand)
    {
        defaultCommand = null;
        return false;
    }

    public static CommandTreeNode FromEntries(List<RegistrationEntry> collectionRegistrationEntries,
        Precondition[] preconditions, CommandDelegate? helpHandler = null)
    {
        var visitor = new RegistrationVisitor(preconditions);
        var helpVisitor = new RegistrationVisitor(preconditions, helpHandler);

        return new CommandTreeNode(
            visitor.VisitAll(collectionRegistrationEntries).ToFrozenDictionary(),
            null,
            null,
            new ModuleMeta("root", [], "Root contains all commands", ""),
            []);
    }
}