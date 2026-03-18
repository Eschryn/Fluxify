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

using System.Text.Json.Serialization;
using Fluxify.Core.Types;

namespace Fluxify.Dto.Users;

public record UserPrivate(
    int? AccentColor,
    string[] Acls,
    UserAuthenticatorTypes[] AuthenticatorTypes,
    [property: JsonPropertyName("avatar")] string? AvatarHash,
    int? AvatarColor,
    [property: JsonPropertyName("banner")] string? BannerHash,
    int? BannerColor,
    string? Bio,
    bool? Bot,
    string Discriminator,
    string? Email,
    bool? EmailBounced,
    PublicUserFlags Flags,
    string? GlobalName,
    bool? HasDismissedPremiumOnboarding,
    bool? HasEverPurchased,
    bool? HasUnreadGiftInventory,
    Snowflake Id,
    bool IsStaff,
    bool MfaEnabled,
    bool NsfwAllowed,
    string? PasswordLastChangedAt,
    UserPrivateResponsePendingBulkMessageDeletion? PendingBulkMessageDeletion,
    string? Phone,
    bool PremiumBadgeHidden,
    bool PremiumBadgeMasked,
    bool PremiumBadgeSequenceHidden,
    bool PremiumBadgeTimestampHidden,
    string? PremiumBillingCycle,
    bool PremiumEnabledOverride,
    int? PremiumLifetimeSequence,
    bool PremiumPurchaseDisabled,
    DateTimeOffset? PremiumSince,
    UserPremiumTypes PremiumType,
    DateTimeOffset? PremiumUntil,
    bool? PremiumWillCancel,
    string? Pronouns,
    string[]? RequiredActions,
    bool? System,
    string[] Traits,
    int UnreadGiftInventoryCount,
    bool UsedMobileClient,
    string Username,
    bool Verified
);