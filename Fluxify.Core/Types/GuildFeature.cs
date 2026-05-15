// Copyright 2026 Fluxify Contributors
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Numerics;
using System.Text.Json.Serialization;

namespace Fluxify.Core.Types;

[JsonConverter(typeof(GuildFeatureConverter))]
public readonly struct GuildFeature 
    : IEqualityOperators<GuildFeature, GuildFeature, bool>, IEquatable<GuildFeature>
{

    private readonly string _value;

    internal GuildFeature(string value)
    {
        _value = value;
    }

    public string Value => _value;

    public static GuildFeature AnimatedIcon => new("ANIMATED_ICON");
    public static GuildFeature AnimatedBanner => new("ANIMATED_BANNER");
    public static GuildFeature Banner => new("BANNER");
    public static GuildFeature DetachedBanner => new("DETACHED_BANNER");
    public static GuildFeature InviteSplash => new("INVITE_SPLASH");
    public static GuildFeature InvitesDisabled => new("INVITES_DISABLED");
    public static GuildFeature TextChannelFlexibleNames => new("TEXT_CHANNEL_FLEXIBLE_NAMES");
    public static GuildFeature MoreEmoji => new("MORE_EMOJI");
    public static GuildFeature MoreStickers => new("MORE_STICKERS");
    public static GuildFeature UnlimitedEmoji => new("UNLIMITED_EMOJI");
    public static GuildFeature UnlimitedStickers => new("UNLIMITED_STICKERS");
    public static GuildFeature ExpressionPurgeAllowed => new("EXPRESSION_PURGE_ALLOWED");
    public static GuildFeature VanityUrl => new("VANITY_URL");
    public static GuildFeature Discoverable => new("DISCOVERABLE");
    public static GuildFeature Partnered => new("PARTNERED");
    public static GuildFeature Verified => new("VERIFIED");
    public static GuildFeature VipVoice => new("VIP_VOICE");
    public static GuildFeature UnavailableHidden => new("UNAVAILABLE_HIDDEN");
    public static GuildFeature UnavailableForEveryone => new("UNAVAILABLE_FOR_EVERYONE");
    public static GuildFeature UnavailableForEveryoneButStaff => new("UNAVAILABLE_FOR_EVERYONE_BUT_STAFF");
    public static GuildFeature Visionary => new("VISIONARY");
    public static GuildFeature Operator => new("OPERATOR");
    public static GuildFeature LargeGuildOverride => new("LARGE_GUILD_OVERRIDE");
    public static GuildFeature VeryLargeGuild => new("VERY_LARGE_GUILD");
    public static GuildFeature RaidDetected => new("RAID_DETECTED");
    public static GuildFeature CloneEmojiDisabled => new("CLONE_EMOJI_DISABLED");
    public static GuildFeature CloneStickerDisabled => new("CLONE_STICKER_DISABLED");
    public static GuildFeature HideOwnerCrown => new("HIDE_OWNER_CROWN");

    // TODO: Will be removed soon
    public static GuildFeature ContentWarningsBackfilled => new("CONTENT_WARNINGS_BACKFILLED");
    
    public static bool operator ==(GuildFeature left, GuildFeature right) => left._value == right._value;
    public static bool operator !=(GuildFeature left, GuildFeature right) => left._value != right._value;
    public bool Equals(GuildFeature other) => _value.Equals(other._value);
    public override bool Equals(object? obj) => obj is GuildFeature other && Equals(other);
    public override int GetHashCode() => _value.GetHashCode();
}