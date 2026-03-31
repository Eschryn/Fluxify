using Fluxify.Core.Types;
using Fluxify.Dto.Common;

namespace Fluxify.Dto.Webhooks;

public record WebhookTokenResponse(
    MediaHash? Avatar,
    Snowflake ChannelId,
    Snowflake GuildId,
    Snowflake Id,
    string Name,
    string Token
);