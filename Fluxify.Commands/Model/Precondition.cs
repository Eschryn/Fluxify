namespace Fluxify.Commands.Model;

public record Precondition(
    string Name,
    string? Description,
    PreconditionDelegate Execute)
{
    public virtual bool Equals(Precondition? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Name == other.Name;
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }
}