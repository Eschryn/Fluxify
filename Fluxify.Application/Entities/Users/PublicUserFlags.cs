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

namespace Fluxify.Application.Entities.Users;

[Flags]
public enum PublicUserFlags : ulong
{
    Staff = 1UL << 0,
    CtpMember = 1UL << 1,
    Partner = 1UL << 2,
    BugHunter= 1UL << 3,
    FriendlyBot= 1UL << 4,
    FriendlyBotManualApproval = 1UL << 5,
    Spammer = 1UL << 6,
    HighGlobalRateLimit = 1UL << 33,
    Deleted = 1UL << 34,
    DisabledSuspiciousActivity = 1UL << 35,
    SelfDeleted = 1UL << 36,
    Disabled = 1UL << 38,
    HasSessionStarted = 1UL << 39,
    RateLimitBypass = 1UL << 47,
    ReportBanned = 1UL << 48,
    VerifiedNotUnderage = 1UL << 49,
    HasDismissedPremiumOnboarding = 1UL << 51,
    AppStoreReviewer = 1UL << 53,
    StaffHidden = 1UL << 57,
    AgeVerifiedAdult = 1UL << 60,
    ForceInboundPhoneVerification = 1UL << 61,
    NotSuspicious =  1UL << 62
}