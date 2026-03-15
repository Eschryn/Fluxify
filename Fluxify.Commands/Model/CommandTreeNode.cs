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
    CommandDelegate? DefaultCommand,
    Precondition[] Preconditions
) {
    public static CommandTreeNode FromEntries(List<RegistrationEntry> collectionRegistrationEntries, Precondition[] preconditions)
    {
        var visitor = new RegistrationVisitor(preconditions);
        return new CommandTreeNode(
            collectionRegistrationEntries.ToFrozenDictionary(k => k.MetaName, visitor.Visit), null, []);
    }
}