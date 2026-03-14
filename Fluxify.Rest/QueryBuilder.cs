using System.Text;

namespace Fluxify.Rest.Channel;

public class QueryBuilder
{
    public StringBuilder QueryStringBuilder { get; } = new("?");
    
    public QueryBuilder AddQuery(string key, string? value)
    {
        if (value != null)
        {
            QueryStringBuilder.Append($"{key}={value}&");
        }
        
        return this;
    }
    
    public static implicit operator string(QueryBuilder builder) 
        => builder.QueryStringBuilder
            .Remove(builder.QueryStringBuilder.Length - 1, 1)
            .ToString();
    
    public override string ToString() => this;

    public QueryBuilder AddOptional(string key, bool add)
    {
        if (add)
        {
            QueryStringBuilder.Append($"{key}&");
        }
        return this;
    }
}