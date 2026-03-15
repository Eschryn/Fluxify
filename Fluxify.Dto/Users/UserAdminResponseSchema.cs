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

using Fluxify.Core.Types;
using Fluxify.Dto.Admin;

namespace Fluxify.Dto.Users;

public record UserAdminResponseSchema(
    int? AccentColor,
    string[] Acls,
    UserAuthenticatorTypes[] AuthenticatorTypes,
    string? Avatar,
    string? Banner,
    string? Bio,
    bool Bot,
    DateTimeOffset DateOfBirth,
    string? DeletionPublicReason,
    int? DeletionReasonCode,
    int Discriminator,
    string? Email,
    bool EmailBounced,
    bool EmailVerified,
    UserFlags Flags,
    string? GlobalName,
    bool HasTotp,
    Snowflake Id,
    DateTimeOffset? LastActiveAt,
    string? LastActiveIp,
    string? LastActiveIpReverse,
    string? LastActiveLocation,
    string? Locale,
    DateTimeOffset? PendingBulkMessageDeletionAt,
    DateTimeOffset? PendingDeletionAt,
    string? Phone,
    DateTimeOffset? PremiumSince,
    int? PremiumType,
    DateTimeOffset? PremiumUntil,
    string? Pronouns,
    SuspiciousActivityFlags SuspiciousActivityFlags,
    bool System,
    string? TempBannedUntil,
    string[] Traits,
    string Username
);