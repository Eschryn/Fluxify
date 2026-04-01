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
using System.Text.Json.Serialization;
using Fluxify.Core.Types;
using Fluxify.Dto.Auth;
using Fluxify.Dto.Users;

namespace Fluxify.Application.Entities.Users;

public class PrivateUser(FluxerApplication fluxerApplication) : GlobalUser(fluxerApplication)
{
    public int? AccentColor { get; set;  }
    public string[] Acls { get; set;  }
    public AuthenticatorTypes[] AuthenticatorTypes { get; set;  }
    public string? BannerHash { get; set;  }
    public Color? BannerColor { get; set;  }
    public string? Bio { get; set;}
    public string? Email { get; set; }
    public bool? EmailBounced { get; set; }
    public bool? HasDismissedPremiumOnboarding { get; set; }
    public bool? HasEverPurchased { get; set; }
    public bool? HasUnreadGiftInventory { get; set; }
    public bool IsStaff { get; set; }
    public bool MfaEnabled { get; set; }
    public bool NsfwAllowed { get; set; }
    public string? PasswordLastChangedAt { get; set; }
    public UserPrivateResponsePendingBulkMessageDeletion? PendingBulkMessageDeletion { get; set; }
    public string? Phone { get; set; }
    public bool PremiumBadgeHidden { get; set; }
    public bool PremiumBadgeMasked { get; set; }
    public bool PremiumBadgeSequenceHidden { get; set; }
    public bool PremiumBadgeTimestampHidden { get; set; }
    public string? PremiumBillingCycle { get; set; }
    public bool PremiumEnabledOverride { get; set; }
    public int? PremiumLifetimeSequence { get; set; }
    public bool PremiumPurchaseDisabled { get; set; }
    public DateTimeOffset? PremiumSince { get; set; }
    public UserPremiumTypes PremiumType { get; set; }
    public DateTimeOffset? PremiumUntil { get; set; }
    public bool? PremiumWillCancel { get; set; }
    public string? Pronouns { get; set; }
    public string[]? RequiredActions { get; set; }
    public string[] Traits { get; set; }
    public int UnreadGiftInventoryCount { get; set; }
    public bool UsedMobileClient { get; set; }
    public bool Verified { get; set; }
}