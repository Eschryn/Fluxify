using System.Globalization;
using System.Text;
using Fluxify.Core.Types;
using Fluxify.Dto.Channels.GroupDm;
using Fluxify.Rest.RequestDtos;

namespace Fluxify.Rest.Channel;

public class CallRequestBuilder(HttpClient client, Snowflake id)
{
    private static readonly CultureInfo FormatProvider = CultureInfo.InvariantCulture;
    private static readonly CompositeFormat CallUrl = CompositeFormat.Parse("channels/{0}/call");
    private static readonly CompositeFormat EndCallUrl = CompositeFormat.Parse("channels/{0}/call/end");
    private static readonly CompositeFormat RingCallUrl = CompositeFormat.Parse("channels/{0}/call/ring");
    private static readonly CompositeFormat StopRingCallUrl = CompositeFormat.Parse("channels/{0}/call/stop-ringing");
    private static string Uri(CompositeFormat format, Snowflake id) => string.Format(FormatProvider, format, id);

    public async Task<CallEligibilityResponse?> GetEligibilityAsync()
        => await client.JsonRequestAsync<CallEligibilityResponse>(
            HttpMethod.Get,
            Uri(CallUrl, id)
        );
    
    public async Task UpdateRegionAsync(UpdateCallRegionRequest request)
        => await client.JsonRequestAsync<UpdateCallRegionRequest>(
            HttpMethod.Patch,
            Uri(CallUrl, id)
        );
    
    public async Task EndCallAsync()
        => await client.RequestAsync(
            HttpMethod.Post,
            Uri(EndCallUrl, id)
        );
    
    public async Task RingAsync(RingRequest request)
        => await client.JsonRequestAsync(
            HttpMethod.Post,
            Uri(RingCallUrl, id),
            request
        );
    
    public async Task StopRingingAsync(RingRequest request)
        => await client.JsonRequestAsync(
            HttpMethod.Post,
            Uri(StopRingCallUrl, id),
            request
        );
}