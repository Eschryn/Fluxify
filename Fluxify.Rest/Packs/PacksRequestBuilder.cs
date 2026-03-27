using Fluxify.Core.Types;

namespace Fluxify.Rest;

public class PacksRequestBuilder(HttpClient httpClient)
{
    public PackRequestBuilder this[Snowflake id] => new(httpClient, id);
}