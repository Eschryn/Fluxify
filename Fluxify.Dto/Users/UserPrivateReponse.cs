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

using System.Drawing;
using Fluxify.Core.Types;
using Fluxify.Dto.Auth;
using Fluxify.Dto.Common;

namespace Fluxify.Dto.Users;

/// <summary>
/// Represents the complete private user object.
/// </summary>
/// <inheritdoc/>
/// <param name="IsStaff">Whether the user has the staff flag.</param>
/// <param name="Acls">All admin access control entries the account holds.</param>
/// <param name="Traits">All account traits.</param>
/// <param name="Email">The email of the account.</param>
/// <param name="EmailBounced">Whether the mail provider rejected the mails.</param>
/// <param name="Phone">Always null.</param>
/// <param name="HasVerifiedPhone">Whether a phone number was verified for this account.</param>
/// <param name="Bio">The profile biography.</param>
/// <param name="Pronouns">The pronouns of the user.</param>
/// <param name="AccentColor">The accent color of the profile.</param>
/// <param name="Timezone">The IANA timezone of the user.</param>
/// <param name="TimezonePrivacyFlags">Defines to who the timezone should be visible.</param>
/// <param name="Banner">The banner of the profile.</param>
/// <param name="BannerColor">Packed banner image color.</param>
/// <param name="MfaEnabled">Whether Mfa has been enabled for this account.</param>
/// <param name="AuthenticatorTypes">Which authentification methods have been set up for this account.</param>
/// <param name="Verified">Whether the email of the account has been verified.</param>
/// <param name="PremiumType">Which model of premium the account has.</param>
/// <param name="PremiumSince">Since when the account has premium. Null if no premium.</param>
/// <param name="PremiumUntil">The point in time when on premium will end if not extended. Null if no premium.</param>
/// <param name="PremiumWillCancel">The user has canceled premium and the subscription will not renew,</param>
/// <param name="PremiumBillingCycle">The chosen billing cycle for the premium type, or null.</param>
/// <param name="PremiumLifetimeSequence">Lifetime premium no., or null.</param>
/// <param name="PremiumGraceEndsAt">Grace period if lifetime ended but for example billing failed.</param>
/// <param name="PremiumDiscriminator">The current discriminator was chosen.</param>
/// <param name="PremiumBadgeHidden">User selected that premium should be hidden in the profile view.</param>
/// <param name="PremiumBadgeMasked">Lifetime premium will be shown as regular premium if true.</param>
/// <param name="PremiumBadgeTimestampHidden">User selected that premium start date will be hidden.</param>
/// <param name="PremiumBadgeSequenceHidden">User selected that lifetime subscription no. will be hidden.</param>
/// <param name="PremiumPurchaseDisabled">If true, premium purchasing has been disabled for this account.</param>
/// <param name="PremiumEnabledOverride">Administrator override grants this account premium entitlements.</param>
/// <param name="PremiumPerksDisabled">Premium entitlements have been suspended for this account.</param>
/// <param name="PasswordLastChangedAt">Date and time of the last password change.</param>
/// <param name="LastVoiceActivitySharingChangedAt">Date and time when the last voice activity changed.</param>
/// <param name="RequiredActions">Always empty.</param>
/// <param name="NsfwAllowed">Whether user can access nsfw flagged content.</param>
/// <param name="HasDismissedPremiumOnboarding">User has dismissed the premium onboarding.</param>
/// <param name="HasEverPurchased">True if the user has ever completed a purchase.</param>
/// <param name="HasUnreadGiftInventory">True if the user has gifts in their inventory that haven't been marked as read.</param>
/// <param name="UnreadGiftInventoryCount">The number of unread gifts in the user's inventory.</param>
/// <param name="PendingBulkMessageDeletion">Information about an upcoming or in process message deletion.</param>
/// <param name="AgeVerifiedAdult">User has completed age verification, null if false.</param>
/// <param name="TermsAgreedAt">Date and time when the user agreed to the most recent ToS</param>
/// <param name="PrivacyAgreedAt">Date and time when the user agreed to the most recent privacy policy.</param>
/// <seealso href="https://docs.fluxer.app/http-api/users/#user-object"/>
public record UserPrivateReponse(
    Snowflake Id,
    string Username,
    string Discriminator,
    string? GlobalName,
    MediaHash? Avatar,
    Color? AvatarColor,
    bool? Bot,
    bool? System,
    PublicUserFlags Flags,
    MentionFlags? MentionFlags,
    bool IsStaff,
    string[] Acls,
    string[] Traits,
    string? Email,
    bool? EmailBounced,
    string? Phone,
    bool HasVerifiedPhone,
    string? Bio,
    string? Pronouns,
    int? AccentColor,
    string? Timezone,
    ProfileFieldPrivacyFlags? TimezonePrivacyFlags,
    MediaHash? Banner,
    Color? BannerColor,
    bool MfaEnabled,
    AuthenticatorTypes?[] AuthenticatorTypes,
    bool Verified,
    UserPremiumTypes? PremiumType,
    DateTimeOffset? PremiumSince,
    DateTimeOffset? PremiumUntil,
    bool? PremiumWillCancel,
    string? PremiumBillingCycle,
    DateTimeOffset? PremiumGraceEndsAt,
    int? PremiumLifetimeSequence,
    bool PremiumDiscriminator,
    bool PremiumBadgeHidden,
    bool PremiumBadgeMasked,
    bool PremiumBadgeTimestampHidden,
    bool PremiumBadgeSequenceHidden,
    bool PremiumPurchaseDisabled,
    bool PremiumEnabledOverride,
    bool PremiumPerksDisabled,
    DateTimeOffset? PasswordLastChangedAt,
    DateTimeOffset? LastVoiceActivitySharingChangedAt,
    [property: Obsolete("Marked as deprecated in API.")]string[]? RequiredActions,
    bool NsfwAllowed,
    bool HasDismissedPremiumOnboarding,
    bool HasEverPurchased,
    bool HasUnreadGiftInventory,
    int UnreadGiftInventoryCount,
    PendingBulkMessageDeletion? PendingBulkMessageDeletion,
    bool? AgeVerifiedAdult,
    DateTimeOffset? TermsAgreedAt,
    DateTimeOffset? PrivacyAgreedAt
) : UserPartialResponse(
    Id,
    Username,
    Discriminator,
    GlobalName,
    Avatar,
    AvatarColor,
    System,
    Bot,
    Flags,
    MentionFlags
);

