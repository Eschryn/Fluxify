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
using Fluxify.Dto.Auth;
using Fluxify.Dto.Users;

namespace Fluxify.Application.Entities.Users;

public class PrivateUser(FluxerApplication fluxerApplication, Snowflake id) : GlobalUser(fluxerApplication, id)
{
    public int? AccentColor { get; internal set;  }
    public string[] Acls { get; internal set; } = [];
    public AuthenticatorTypes[] AuthenticatorTypes { get; internal set;  } = [];
    public string? BannerHash { get; internal set;  }
    public Color? BannerColor { get; internal set;  }
    public string? Bio { get; internal set;}
    public string? Email { get; internal set; }
    public bool? EmailBounced { get; internal set; }
    public bool? HasDismissedPremiumOnboarding { get; internal set; }
    public bool? HasEverPurchased { get; internal set; }
    public bool? HasUnreadGiftInventory { get; internal set; }
    public bool IsStaff { get; internal set; }
    public bool MfaEnabled { get; internal set; }
    public bool NsfwAllowed { get; internal set; }
    public string? PasswordLastChangedAt { get; internal set; }
    public UserPrivateResponsePendingBulkMessageDeletion? PendingBulkMessageDeletion { get; internal set; }
    public string? Phone { get; internal set; }
    public bool PremiumBadgeHidden { get; internal set; }
    public bool PremiumBadgeMasked { get; internal set; }
    public bool PremiumBadgeSequenceHidden { get; internal set; }
    public bool PremiumBadgeTimestampHidden { get; internal set; }
    public string? PremiumBillingCycle { get; internal set; }
    public bool PremiumEnabledOverride { get; internal set; }
    public int? PremiumLifetimeSequence { get; internal set; }
    public bool PremiumPurchaseDisabled { get; internal set; }
    public DateTimeOffset? PremiumSince { get; internal set; }
    public UserPremiumTypes PremiumType { get; internal set; }
    public DateTimeOffset? PremiumUntil { get; internal set; }
    public bool? PremiumWillCancel { get; internal set; }
    public string? Pronouns { get; internal set; }
    public string[]? RequiredActions { get; internal set; }
    public string[] Traits { get; internal set; } = [];
    public int UnreadGiftInventoryCount { get; internal set; }
    public bool UsedMobileClient { get; internal set; }
    public bool Verified { get; internal set; }
}