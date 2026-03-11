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