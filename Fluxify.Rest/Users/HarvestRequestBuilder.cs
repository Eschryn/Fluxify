using System.Globalization;
using System.Text;
using Fluxify.Core.Types;
using Fluxify.Dto.Users.DataHarvest;

namespace Fluxify.Rest.Users;

public class HarvestRequestBuilder(HttpClient client)
{
    private static readonly IFormatProvider FormatProvider = CultureInfo.InvariantCulture;
    private const string HarvestUrl = "users/@me/harvest";
    private const string LatestUrl = "users/@me/harvest/latest";
    private static readonly CompositeFormat HarvestIdUrl = CompositeFormat.Parse("users/@me/harvest/{0}");
    private static readonly CompositeFormat HarvestIdDownloadUrl = CompositeFormat.Parse("users/@me/harvest/{0}/download");
    
    public async Task<HarvestCreationResponseSchema?> RequestAsync(
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<HarvestCreationResponseSchema>(
        HttpMethod.Post,
        HarvestUrl,
        cancellationToken: cancellationToken
    );
    
    public async Task<HarvestStatusResponseSchema?> GetLatestAsync(
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<HarvestStatusResponseSchema>(
        HttpMethod.Get,
        LatestUrl,
        cancellationToken: cancellationToken
    );
    
    public async Task<HarvestStatusResponseSchema?> GetAsync(
        Snowflake harvestId,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<HarvestStatusResponseSchema>(
        HttpMethod.Get,
        string.Format(FormatProvider, HarvestIdUrl, harvestId),
        cancellationToken: cancellationToken
    );
    
    public async Task<HarvestDownloadUrlResponse?> GetDownloadAsync(
        Snowflake harvestId,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<HarvestDownloadUrlResponse>(
        HttpMethod.Get,
        string.Format(FormatProvider, HarvestIdDownloadUrl, harvestId),
        cancellationToken: cancellationToken
    );
}