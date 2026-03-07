using System.Collections.Frozen;

namespace Fluxify.Commands;

internal static class RegistrationVisitor
{
    public static CommandTreeNode Visit(RegistrationEntry entry)
    {
        return entry switch
        {
            CommandRegistration reg => new CommandTreeNode(null, reg.Handler),
            ModuleRegistration reg => new CommandTreeNode(
                reg.Children.ToFrozenDictionary(k => k.MetaName, Visit), null),
            _ => throw new InvalidOperationException("Unknown registration entry type")
        };
    }
}