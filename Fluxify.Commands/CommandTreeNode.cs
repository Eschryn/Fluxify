using System.Collections.Frozen;

namespace Fluxify.Commands;

internal record CommandTreeNode(
    FrozenDictionary<string, CommandTreeNode> Commands,
    CommandDelegate? DefaultCommand)
{
    public static CommandTreeNode FromEntries(List<RegistrationEntry> collectionRegistrationEntries)
    {
        return new CommandTreeNode(
            collectionRegistrationEntries.ToFrozenDictionary(k => k.MetaName, RegistrationVisitor.Visit), null);
    }
}