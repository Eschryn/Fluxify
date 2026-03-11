namespace Fluxify.Commands.Model;

// preconditions should not take long
public delegate PreconditionResult PreconditionDelegate(CommandContext context);