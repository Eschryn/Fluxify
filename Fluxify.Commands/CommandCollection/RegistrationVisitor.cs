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
            CommandRegistration reg => new CommandTreeNode(Empty, reg.Handler, reg.Preconditions.Select(p => _preconditions[p]).ToArray()),
            ModuleRegistration reg => new CommandTreeNode(
                reg.Children.ToFrozenDictionary(k => k.MetaName, Visit), null,  reg.Preconditions.Select(p => _preconditions[p]).ToArray()),
            _ => throw new InvalidOperationException("Unknown registration entry type")
        };
    }

    private static readonly FrozenDictionary<string, CommandTreeNode> Empty = FrozenDictionary.Create<string, CommandTreeNode>();
}