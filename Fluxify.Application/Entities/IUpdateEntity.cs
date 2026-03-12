namespace Fluxify.Application.Entities;

public interface IUpdateEntity<TData>
{
    public TData UpdateEntity(TData data, TData update);
}