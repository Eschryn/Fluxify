using Fluxify.Core.Types;
using Fluxify.Dto.Channels.GroupDm;
using Fluxify.Dto.Common;
using Fluxify.Dto.Packs;
using Fluxify.Dto.Users.Relationships;

namespace Fluxify.Dto.Users.Settings;

public record UserSettingsUpdateRequest(
    long? AfkTimeout,
    bool? AnimateEmoji,
    StickerAnimationOptions? AnimateStickers,
    bool? BotDefaultGuildsRestricted,
    Snowflake[]? BotRestrictedGuilds,
    CustomStatus? CustomStatus,
    bool? DefaultGuildsRestricted,
    bool? DefaultHideMutedChannels,
    bool? DeveloperMode,
    FriendSourceFlags? FriendSourceFlags,
    bool? GifAutoPlay,
    GroupDmAddPermissionFlags? GroupDmAddPermissionFlags,
    UserSettingsResponseGuildFoldersItem[]? GuildFolders,
    IncomingCallFlags? IncomingCallFlags,
    bool? InlineAttachmentMedia,
    bool? InlineEmbedMedia,
    Locale? Locale,
    bool? MessageDisplayCompact,
    bool? RenderEmbeds,
    bool? RenderReactions,
    bool? RenderSpoilers,
    Snowflake[]? RestrictedGuilds,
    string? Status,
    DateTimeOffset? StatusResetsAt,
    string? StatusResetsTo,
    string? Theme,
    TimeFormatTypes? TimeFormat,
    string[]? TrustedDomains
);