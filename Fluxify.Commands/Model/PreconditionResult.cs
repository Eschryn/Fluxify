namespace Fluxify.Commands.Model;

public record PreconditionResult(
    bool IsSuccess,
    string? Message 
)
{
    public static PreconditionResult Success { get; } = new PreconditionResult(true, null);
}