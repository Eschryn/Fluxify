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
using Fluxify.Dto.Channels;
using Fluxify.Dto.Channels.GroupDm;
using Fluxify.Dto.Channels.Text.Messages;
using Fluxify.Dto.Channels.Text.Messages.Pin;
using Fluxify.Dto.Channels.Voice;
using Fluxify.Dto.Common;
using Fluxify.Dto.Gateway;
using Fluxify.Dto.Guilds;
using Fluxify.Dto.Guilds.AuditLog;
using Fluxify.Dto.Guilds.Emoji;
using Fluxify.Dto.Guilds.Invite;
using Fluxify.Dto.Guilds.Members;
using Fluxify.Dto.Guilds.Members.Search;
using Fluxify.Dto.Guilds.Roles;
using Fluxify.Dto.Guilds.Settings;
using Fluxify.Dto.Guilds.Stickers;
using Fluxify.Dto.OAuth2;
using Fluxify.Dto.SavedMedia;
using Fluxify.Dto.Users;
using Fluxify.Dto.Users.DataHarvest;
using Fluxify.Dto.Users.GuildSettings;
using Fluxify.Dto.Users.Push;
using Fluxify.Dto.Users.Relationships;
using Fluxify.Dto.Users.ScheduledMessages;
using Fluxify.Dto.Users.Settings;
using Fluxify.Dto.Users.Settings.EmailChange;
using Fluxify.Dto.Users.Settings.PasswordChange;
using Fluxify.Dto.Users.Settings.PhoneChange;
using Fluxify.Dto.Users.Settings.Security;
using Fluxify.Dto.Users.Settings.Security.Mfa;
using Fluxify.Dto.Users.Settings.Security.Webauth;

namespace Fluxify.Dto.Json;

[JsonSourceGenerationOptions(
    NumberHandling = JsonNumberHandling.AllowReadingFromString
)]
[JsonSerializable(typeof(Snowflake))]
[JsonSerializable(typeof(Snowflake[]))]
[JsonSerializable(typeof(UserPrivate))]
[JsonSerializable(typeof(UserSessionResponse))]
[JsonSerializable(typeof(UserGuildSettingsResponse))]
[JsonSerializable(typeof(UserGuildSettingsRequest))]
[JsonSerializable(typeof(FavoriteMemeResponse))]
[JsonSerializable(typeof(GuildResponse))]
[JsonSerializable(typeof(GuildMemberResponse))]
[JsonSerializable(typeof(ChannelResponse))]
[JsonSerializable(typeof(RelationshipResponse))]
[JsonSerializable(typeof(UserSettings))]
[JsonSerializable(typeof(MessageResponse))]
[JsonSerializable(typeof(CreateMessageRequest))]
[JsonSerializable(typeof(UpdateMessageRequest))]
[JsonSerializable(typeof(ChannelCreateRequest))]
[JsonSerializable(typeof(ChannelUpdateRequest))]
[JsonSerializable(typeof(UpdateCallRegionRequest))]
[JsonSerializable(typeof(RingRequest))]
[JsonSerializable(typeof(UserPartialResponse[]))]
[JsonSerializable(typeof(MessageResponse[]))]
[JsonSerializable(typeof(ScheduleMessageResponseSchema))]
[JsonSerializable(typeof(ScheduledMessageResponseSchemaPayload))]
[JsonSerializable(typeof(ChannelPinsResponse))]
[JsonSerializable(typeof(CallEligibilityResponse))]
[JsonSerializable(typeof(UpdateCallRegionRequest))]
[JsonSerializable(typeof(RingRequest))]
[JsonSerializable(typeof(ChannelPermissionOverwrite))]
[JsonSerializable(typeof(RtcRegion[]))]
[JsonSerializable(typeof(EmailChangeRequestNewResponse))]
[JsonSerializable(typeof(EmailChangeBouncedRequestNewRequest))]
[JsonSerializable(typeof(EmailChangeTicketRequest))]
[JsonSerializable(typeof(EmailChangeBouncedRequestVerifyNewRequest))]
[JsonSerializable(typeof(UserPrivate))]
[JsonSerializable(typeof(EmailChangeRequestNewResponse))]
[JsonSerializable(typeof(EmailChangeRequestNewRequest))]
[JsonSerializable(typeof(EmailChangeStartResponse))]
[JsonSerializable(typeof(EmailTokenResponse))]
[JsonSerializable(typeof(EmailChangeVerifyNewRequest))]
[JsonSerializable(typeof(EmailChangeVerifyOriginalResponse))]
[JsonSerializable(typeof(EmailChangeVerifyOriginalRequest))]
[JsonSerializable(typeof(HarvestCreationResponseSchema))]
[JsonSerializable(typeof(HarvestStatusResponseSchema))]
[JsonSerializable(typeof(HarvestDownloadUrlResponse))]
[JsonSerializable(typeof(MfaBackupCodesResponse))]
[JsonSerializable(typeof(MfaBackupCodesRequest))]
[JsonSerializable(typeof(SudoVerificationSchema))]
[JsonSerializable(typeof(DisableTotpRequest))]
[JsonSerializable(typeof(EnableMfaTotpRequest))]
[JsonSerializable(typeof(WebAuthnCredentialsResponse))]
[JsonSerializable(typeof(WebAuthnRegisterRequest))]
[JsonSerializable(typeof(WebAuthnChallengeResponse))]
[JsonSerializable(typeof(WebAuthnCredentialUpdateRequest))]
[JsonSerializable(typeof(UserNoteResponse))]
[JsonSerializable(typeof(UserNoteUpdateRequest))]
[JsonSerializable(typeof(Dictionary<Snowflake, string>))]
[JsonSerializable(typeof(PasswordChangeCompleteRequest))]
[JsonSerializable(typeof(PasswordChangeTicketRequest))]
[JsonSerializable(typeof(PasswordChangeStartResponse))]
[JsonSerializable(typeof(PasswordChangeVerifyResponse))]
[JsonSerializable(typeof(PasswordChangeVerifyRequest))]
[JsonSerializable(typeof(PhoneAddRequest))]
[JsonSerializable(typeof(PhoneSendVerificationRequest))]
[JsonSerializable(typeof(PhoneVerifyResponse))]
[JsonSerializable(typeof(PhoneVerifyRequest))]
[JsonSerializable(typeof(CreatePrivateChannelRequest))]
[JsonSerializable(typeof(Dictionary<Snowflake, MessageResponse>))]
[JsonSerializable(typeof(PushSubscribeResponse))]
[JsonSerializable(typeof(PushSubscribeRequest))]
[JsonSerializable(typeof(PushSubscriptionListResponse))]
[JsonSerializable(typeof(SuccessResponse))]
[JsonSerializable(typeof(RelationshipResponse))]
[JsonSerializable(typeof(RelationshipTypePutRequest))]
[JsonSerializable(typeof(RelationshipNicknameUpdateRequest))]
[JsonSerializable(typeof(RelationshipResponse[]))]
[JsonSerializable(typeof(FriendRequestByTagRequest))]
[JsonSerializable(typeof(SavedMessageEntryResponse[]))]
[JsonSerializable(typeof(SaveMessageRequest))]
[JsonSerializable(typeof(ScheduleMessageResponseSchema[]))]
[JsonSerializable(typeof(UpdateScheduledMessageRequest))]
[JsonSerializable(typeof(SudoMfaMethodsResponse))]
[JsonSerializable(typeof(SudoMfaMethodsResponse))]
[JsonSerializable(typeof(WebAuthnChallengeResponse))]
[JsonSerializable(typeof(UserProfileFullResponse))]
[JsonSerializable(typeof(UserPartialResponse))]
[JsonSerializable(typeof(UserTagCheckResponse))]
[JsonSerializable(typeof(UserTagCheckRequest))]
[JsonSerializable(typeof(UserUpdateWithVerificationRequest))]
[JsonSerializable(typeof(GiftCodeMetadataResponse[]))]
[JsonSerializable(typeof(UserGuildSettingsResponse))]
[JsonSerializable(typeof(MessageResponse[]))]
[JsonSerializable(typeof(UserSettingsUpdateRequest))]
[JsonSerializable(typeof(GuildEmojiResponse))]
[JsonSerializable(typeof(GuildEmojiCreateRequest))]
[JsonSerializable(typeof(GuildEmojiBulkCreateRequest))]
[JsonSerializable(typeof(GuildEmojiBulkCreateResponse))]
[JsonSerializable(typeof(GuildUpdateRequest))]
[JsonSerializable(typeof(GuildAuditLogListResponse))]
[JsonSerializable(typeof(ChannelResponse[]))]
[JsonSerializable(typeof(ChannelPositionUpdateRequest))]
[JsonSerializable(typeof(GuildBanResponse))]
[JsonSerializable(typeof(EnabledRequest))]
[JsonSerializable(typeof(GuildTransferOwnershipRequest))]
[JsonSerializable(typeof(GuildVanityUrlResponse))]
[JsonSerializable(typeof(GuildVanityUrlUpdateRequest))]
[JsonSerializable(typeof(GuildResponse[]))]
[JsonSerializable(typeof(GuildMemberResponse))]
[JsonSerializable(typeof(GuildMemberUpdateRequest))]
[JsonSerializable(typeof(GuildMemberResponse[]))]
[JsonSerializable(typeof(GuildMemberSearchResponse))]
[JsonSerializable(typeof(GuildMemberSearchRequest))]
[JsonSerializable(typeof(MyGuildMemberUpdateRequest))]
[JsonSerializable(typeof(GuildRoleResponse[]))]
[JsonSerializable(typeof(GuildRoleCreateRequest))]
[JsonSerializable(typeof(GuildRolePositionItem))]
[JsonSerializable(typeof(GuildRoleHoistPositionItem))]
[JsonSerializable(typeof(GuildRoleUpdateRequest))]
[JsonSerializable(typeof(GuildStickerResponse[]))]
[JsonSerializable(typeof(GuildStickerResponse))]
[JsonSerializable(typeof(GuildStickerCreateBulkResponse))]
[JsonSerializable(typeof(GuildStickerBulkCreateRequest))]
[JsonSerializable(typeof(GuildStickerUpdateRequest))]
[JsonSerializable(typeof(GatewayBotResponse))]
[JsonSerializable(typeof(OAuth2MeResponse))]
[JsonSerializable(typeof(OAuth2AuthorizationResponse[]))]
[JsonSerializable(typeof(ApplicationResponse))]
[JsonSerializable(typeof(ApplicationsMeResponse[]))]
[JsonSerializable(typeof(OAuth2MeResponseApplication[]))]
[JsonSerializable(typeof(ApplicationResponse))]
[JsonSerializable(typeof(ApplicationUpdateRequest))]
[JsonSerializable(typeof(ApplicationCreateRequest))]
[JsonSerializable(typeof(BotProfileResponse))]
[JsonSerializable(typeof(BotProfileUpdateRequest))]
[JsonSerializable(typeof(BotTokenResetResponse))]
[JsonSerializable(typeof(ApplicationPublicResponse))]
public partial class DtoJsonContext : JsonSerializerContext;