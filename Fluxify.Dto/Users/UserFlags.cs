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

namespace Fluxify.Dto.Users;

[Flags]
public enum UserFlags : ulong
{
    Staff = 1,
    CtpMember = 2,
    Partner = 4,
    BugHunter = 8,
    HighGlobalRateLimit = 8589934592,
    FriendlyBot = 16,
    FriendlyBotManualApproval = 32,
    Deleted = 17179869184,
    DisabledSuspiciousActivity = 34359738368,
    SelfDeleted = 68719476736,
    PremiumDiscriminator = 137438953472,
    Disabled = 274877906944,
    HasSessionStarted = 549755813888,
    PremiumBadgeHidden = 1099511627776,
    PremiumBadgeMasked = 2199023255552,
    PremiumBadgeTimestampHidden = 4398046511104,
    PremiumBadgeSequenceHidden = 8796093022208,
    PremiumPerksSanitized = 17592186044416,
    PremiumPurchaseDisabled = 35184372088832,
    PremiumEnabledOverride = 70368744177664,
    RateLimitBypass = 140737488355328,
    ReportBanned = 281474976710656,
    VerifiedNotUnderage = 562949953421312,
    HasDismissedPremiumOnboarding = 2251799813685248,
    UsedMobileClient = 4503599627370496,
    AppStoreReviewer = 9007199254740992,
    DmHistoryBackfilled = 18014398509481984,
    HasRelationshipsIndexed = 36028797018963968,
    MessagesByAuthorBackfilled = 36028797018963968,
    StaffHidden = 144115188075855872,
    BotSanitized = 288230376151711744
}