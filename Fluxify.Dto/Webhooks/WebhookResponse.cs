using Fluxify.Core.Types;
using Fluxify.Dto.Common;
using Fluxify.Dto.Users;

namespace Fluxify.Dto.Webhooks;

public record WebhookResponse(
    MediaHash? Avatar,
    Snowflake ChannelId,
    Snowflake GuildId,
    Snowflake Id,
    string Name,
    string Token,
    UserPartialResponse User
) : WebhookTokenResponse(
    Avatar,
    ChannelId,
    GuildId,
    Id,
    Name,
    Token
);