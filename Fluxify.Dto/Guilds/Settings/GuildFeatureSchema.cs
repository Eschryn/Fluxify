using System.Text.Json.Serialization;

namespace Fluxify.Dto.Guilds.Settings;

[JsonConverter(typeof(JsonStringEnumConverter<GuildFeatureSchema>))]
public enum GuildFeatureSchema
{
    AnimatedIcon,
    AnimatedBanner,
    Banner,
    DetachedBanner,
    InviteSplash,
    InvitesDisabled,
    TextChannelFlexibleNames,
    MoreEmoji,
    MoreStickers,
    UnlimitedEmoji,
    UnlimitedStickers,
    ExpressionPurgeAllowed,
    VanityUrl,
    Verified,
    VipVoice,
    UnavailableForEveryone,
    UnavailableForEveryoneButStaff,
    Visionary,
    Operator,
    LargeGuildOverride,
    VeryLargeGuild,
}