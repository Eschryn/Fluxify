using Fluxify.Dto.Gateway;

namespace Fluxify.Rest;

public class GatewayRequestBuilder(HttpClient client)
{
    private const string BotUrl = "gateway/bot";
    
    public async Task<GatewayBotResponse?> GetGatewayBotAsync(
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<GatewayBotResponse>(
        HttpMethod.Get,
        BotUrl,
        cancellationToken: cancellationToken
    );
}