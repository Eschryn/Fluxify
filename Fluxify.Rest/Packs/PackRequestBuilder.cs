using System.Globalization;
using System.Text;
using Fluxify.Core.Types;
using Fluxify.Dto.Invites;
using Fluxify.Dto.Packs;

namespace Fluxify.Rest.Packs;

public class PackRequestBuilder(HttpClient httpClient, Snowflake id)
{
    private static readonly IFormatProvider FormatProvider = CultureInfo.InvariantCulture;
    private static readonly CompositeFormat PackUrl = CompositeFormat.Parse("packs/{0}");

    public Task<InviteMetadataResponseSchema[]?> ListInvitesAsync(
        CancellationToken cancellationToken = default
    ) => httpClient.JsonRequestAsync<InviteMetadataResponseSchema[]>(
        HttpMethod.Get,
        string.Format(FormatProvider, PackUrl, id),
        cancellationToken: cancellationToken
    );

    public Task<InviteMetadataResponseSchema?> CreateInviteAsync(
        PackInviteCreateRequest request,
        CancellationToken cancellationToken = default
    ) => httpClient.JsonRequestAsync<PackInviteCreateRequest, InviteMetadataResponseSchema>(
        HttpMethod.Post,
        string.Format(FormatProvider, PackUrl, id),
        request,
        cancellationToken: cancellationToken
    );
}