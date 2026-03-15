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

namespace Fluxify.Dto;

/// <summary>
/// Errors that can occur during validation
/// </summary>
[JsonConverter(typeof(JsonUpperCaseStringEnumConverter<ValidationErrorCodeSchema>))]
public enum ValidationErrorCodeSchema
{
	/// <summary>
	/// Accent color has been changed too many times recently
	/// </summary>
	AccentColorChangedTooManyTimes,
	/// <summary>
	/// Account is already verified
	/// </summary>
	AccountAlreadyVerified,
	/// <summary>
	/// AFK channel must be in the same guild
	/// </summary>
	AfkChannelMustBeInGuild,
	/// <summary>
	/// AFK channel must be a voice channel
	/// </summary>
	AfkChannelMustBeVoice,
	/// <summary>
	/// All channels must belong to the same guild
	/// </summary>
	AllChannelsMustBelongToGuild,
	/// <summary>
	/// Animated avatars require premium
	/// </summary>
	AnimatedAvatarsRequirePremium,
	/// <summary>
	/// Animated guild banners require the feature to be enabled
	/// </summary>
	AnimatedGuildBannerRequiresFeature,
	/// <summary>
	/// At least one entry is required
	/// </summary>
	AtLeastOneEntryIsRequired,
	/// <summary>
	/// At least one recipient is required
	/// </summary>
	AtLeastOneRecipientRequired,
	/// <summary>
	/// Attachment fields are required
	/// </summary>
	AttachmentFieldsRequired,
	/// <summary>
	/// Attachment ID was not found in the message
	/// </summary>
	AttachmentIdNotFoundInMessage,
	/// <summary>
	/// Attachment IDs must be valid integers
	/// </summary>
	AttachmentIdsMustBeValidIntegers,
	/// <summary>
	/// Attachment metadata provided without files
	/// </summary>
	AttachmentMetadataWithoutFiles,
	/// <summary>
	/// Attachment must be an image
	/// </summary>
	AttachmentMustBeImage,
	/// <summary>
	/// Attachment metadata is required when uploading files
	/// </summary>
	AttachmentsMetadataRequiredWhenUploading,
	/// <summary>
	/// Attachments are not allowed for this message type
	/// </summary>
	AttachmentsNotAllowedForMessage,
	/// <summary>
	/// Avatar has been changed too many times recently
	/// </summary>
	AvatarChangedTooManyTimes,
	/// <summary>
	/// Banner has been changed too many times recently
	/// </summary>
	BannerChangedTooManyTimes,
	/// <summary>
	/// Banners require premium
	/// </summary>
	BannersRequirePremium,
	/// <summary>
	/// Invalid base64 length
	/// </summary>
	Base64LengthInvalid,
	/// <summary>
	/// Bio has been changed too many times recently
	/// </summary>
	BioChangedTooManyTimes,
	/// <summary>
	/// Bucket is required
	/// </summary>
	BucketIsRequired,
	/// <summary>
	/// Cannot add yourself to a group DM
	/// </summary>
	CannotAddYourselfToGroupDm,
	/// <summary>
	/// Cannot delete more than 100 messages at once
	/// </summary>
	CannotDeleteMoreThan100Messages,
	/// <summary>
	/// Cannot send a direct message to yourself
	/// </summary>
	CannotDmYourself,
	/// <summary>
	/// Users with MANAGE_MESSAGES can only edit attachment descriptions, not other metadata
	/// </summary>
	CannotEditAttachmentMetadata,
	/// <summary>
	/// Cannot leave guild as the owner
	/// </summary>
	CannotLeaveGuildAsOwner,
	/// <summary>
	/// Cannot position a channel relative to itself
	/// </summary>
	CannotPositionChannelRelativeToItself,
	/// <summary>
	/// Cannot preload more than 100 channels
	/// </summary>
	CannotPreloadMoreThan100Channels,
	/// <summary>
	/// Cannot reference attachments without providing attachments
	/// </summary>
	CannotReferenceAttachmentsWithoutAttachments,
	/// <summary>
	/// Cannot reorder the 'everyone' role
	/// </summary>
	CannotReorderEveryoneRole,
	/// <summary>
	/// Cannot reply to a system message
	/// </summary>
	CannotReplyToSystemMessage,
	/// <summary>
	/// Cannot set hoist for the 'everyone' role
	/// </summary>
	CannotSetHoistForEveryoneRole,
	/// <summary>
	/// Cannot specify both before and after parameters
	/// </summary>
	CannotSpecifyBothBeforeAndAfter,
	/// <summary>
	/// Cannot use the same role as preceding
	/// </summary>
	CannotUseSameRoleAsPreceding,
	/// <summary>
	/// Categories cannot have a parent channel
	/// </summary>
	CategoriesCannotHaveParentChannel,
	/// <summary>
	/// Categories cannot have parents
	/// </summary>
	CategoriesCannotHaveParents,
	/// <summary>
	/// Changing discriminator requires premium
	/// </summary>
	ChangingDiscriminatorRequiresPremium,
	/// <summary>
	/// Channel does not exist
	/// </summary>
	ChannelDoesNotExist,
	/// <summary>
	/// Channel ID is required
	/// </summary>
	ChannelIdIsRequired,
	/// <summary>
	/// Channel must be a DM or group DM
	/// </summary>
	ChannelMustBeDmOrGroupDm,
	/// <summary>
	/// Channel must be a voice channel
	/// </summary>
	ChannelMustBeVoice,
	/// <summary>
	/// Channel name is empty after normalisation
	/// </summary>
	ChannelNameEmptyAfterNormalization,
	/// <summary>
	/// Channel was not found
	/// </summary>
	ChannelNotFound,
	/// <summary>
	/// Colour value is too high
	/// </summary>
	ColorValueTooHigh,
	/// <summary>
	/// Colour value is too low
	/// </summary>
	ColorValueTooLow,
	/// <summary>
	/// Content exceeds maximum length
	/// </summary>
	ContentExceedsMaxLength,
	/// <summary>
	/// Context channel or guild ID is required
	/// </summary>
	ContextChannelOrGuildIdRequired,
	/// <summary>
	/// Custom emoji was not found
	/// </summary>
	CustomEmojiNotFound,
	/// <summary>
	/// Custom emojis require premium when used outside their source
	/// </summary>
	CustomEmojisRequirePremiumOutsideSource,
	/// <summary>
	/// Custom sticker was not found
	/// </summary>
	CustomStickerNotFound,
	/// <summary>
	/// Custom stickers in DMs require premium
	/// </summary>
	CustomStickersInDmsRequirePremium,
	/// <summary>
	/// Custom stickers require premium when used outside their source
	/// </summary>
	CustomStickersRequirePremiumOutsideSource,
	/// <summary>
	/// Discriminator has an invalid format
	/// </summary>
	DiscriminatorInvalidFormat,
	/// <summary>
	/// Discriminator is out of valid range
	/// </summary>
	DiscriminatorOutOfRange,
	/// <summary>
	/// Duplicate attachment IDs are not allowed
	/// </summary>
	DuplicateAttachmentIdsNotAllowed,
	/// <summary>
	/// Duplicate file index
	/// </summary>
	DuplicateFileIndex,
	/// <summary>
	/// Duplicate recipients are not allowed
	/// </summary>
	DuplicateRecipientsNotAllowed,
	/// <summary>
	/// Voice message attachment must be audio
	/// </summary>
	VoiceMessagesAttachmentMustBeAudio,
	/// <summary>
	/// Voice message attachment waveform is required
	/// </summary>
	VoiceMessagesAttachmentWaveformRequired,
	/// <summary>
	/// Voice message attachment duration is required
	/// </summary>
	VoiceMessagesAttachmentDurationRequired,
	/// <summary>
	/// Voice messages cannot have content
	/// </summary>
	VoiceMessagesCannotHaveContent,
	/// <summary>
	/// Voice messages cannot have embeds
	/// </summary>
	VoiceMessagesCannotHaveEmbeds,
	/// <summary>
	/// Voice messages cannot have favourite memes
	/// </summary>
	VoiceMessagesCannotHaveFavoriteMemes,
	/// <summary>
	/// Voice messages cannot have stickers
	/// </summary>
	VoiceMessagesCannotHaveStickers,
	/// <summary>
	/// Voice message duration exceeds limit
	/// </summary>
	VoiceMessagesDurationExceedsLimit,
	/// <summary>
	/// Voice messages require a single attachment
	/// </summary>
	VoiceMessagesRequireSingleAttachment,
	/// <summary>
	/// Email address is already in use
	/// </summary>
	EmailAlreadyInUse,
	/// <summary>
	/// Email address is required
	/// </summary>
	EmailIsRequired,
	/// <summary>
	/// Email address length is invalid
	/// </summary>
	EmailLengthInvalid,
	/// <summary>
	/// Email must be changed via verification token
	/// </summary>
	EmailMustBeChangedViaToken,
	/// <summary>
	/// Email verification token has expired
	/// </summary>
	EmailTokenExpired,
	/// <summary>
	/// Embed index is out of bounds
	/// </summary>
	EmbedIndexOutOfBounds,
	/// <summary>
	/// Embed splash requires the feature to be enabled
	/// </summary>
	EmbedSplashRequiresFeature,
	/// <summary>
	/// Embeds exceed maximum character count
	/// </summary>
	EmbedsExceedMaxCharacters,
	/// <summary>
	/// Emoji requires guild or pack access
	/// </summary>
	EmojiRequiresGuildOrPackAccess,
	/// <summary>
	/// Failed to parse multipart form data
	/// </summary>
	FailedToParseMultipartFormData,
	/// <summary>
	/// Failed to parse multipart payload
	/// </summary>
	FailedToParseMultipartPayload,
	/// <summary>
	/// Failed to upload image
	/// </summary>
	FailedToUploadImage,
	/// <summary>
	/// Favourite meme name is required
	/// </summary>
	FavoriteMemeNameRequired,
	/// <summary>
	/// Favourite meme was not found
	/// </summary>
	FavoriteMemeNotFound,
	/// <summary>
	/// File index exceeds maximum
	/// </summary>
	FileIndexExceedsMaximum,
	/// <summary>
	/// File not found for scanning
	/// </summary>
	FileNotFoundForScanning,
	/// <summary>
	/// File was not found
	/// </summary>
	FileNotFound,
	/// <summary>
	/// Filename is empty after normalisation
	/// </summary>
	FilenameEmptyAfterNormalization,
	/// <summary>
	/// Filename contains invalid characters
	/// </summary>
	FilenameInvalidCharacters,
	/// <summary>
	/// Filename length is invalid
	/// </summary>
	FilenameLengthInvalid,
	/// <summary>
	/// Filename mismatch for attachment
	/// </summary>
	FilenameMismatchForAttachment,
	/// <summary>
	/// Forward messages cannot contain content
	/// </summary>
	ForwardMessagesCannotContainContent,
	/// <summary>
	/// Forward reference requires channel and message
	/// </summary>
	ForwardReferenceRequiresChannelAndMessage,
	/// <summary>
	/// Display name cannot contain reserved terms
	/// </summary>
	GlobalNameCannotContainReservedTerms,
	/// <summary>
	/// Display name length is invalid
	/// </summary>
	GlobalNameLengthInvalid,
	/// <summary>
	/// Display name is a reserved value
	/// </summary>
	GlobalNameReservedValue,
	/// <summary>
	/// Guild banner requires the feature to be enabled
	/// </summary>
	GuildBannerRequiresFeature,
	/// <summary>
	/// Guild ID must match referenced message
	/// </summary>
	GuildIdMustMatchReferencedMessage,
	/// <summary>
	/// Image size exceeds limit
	/// </summary>
	ImageSizeExceedsLimit,
	/// <summary>
	/// Integer is out of 64-bit range
	/// </summary>
	IntegerOutOfInt64Range,
	/// <summary>
	/// Snowflake is out of valid range
	/// </summary>
	SnowflakeOutOfRange,
	/// <summary>
	/// Invalid audit log reason
	/// </summary>
	InvalidAuditLogReason,
	/// <summary>
	/// Invalid base64 format
	/// </summary>
	InvalidBase64Format,
	/// <summary>
	/// Invalid channel ID
	/// </summary>
	InvalidChannelId,
	/// <summary>
	/// Invalid channel
	/// </summary>
	InvalidChannel,
	/// <summary>
	/// Invalid code
	/// </summary>
	InvalidCode,
	/// <summary>
	/// Invalid current password
	/// </summary>
	InvalidCurrentPassword,
	/// <summary>
	/// Invalid date of birth format
	/// </summary>
	InvalidDateOfBirthFormat,
	/// <summary>
	/// Invalid datetime for scheduled send
	/// </summary>
	InvalidDatetimeForScheduledSend,
	/// <summary>
	/// Invalid email address
	/// </summary>
	InvalidEmailAddress,
	/// <summary>
	/// Invalid email format
	/// </summary>
	InvalidEmailFormat,
	/// <summary>
	/// Invalid email local part
	/// </summary>
	InvalidEmailLocalPart,
	/// <summary>
	/// Invalid email or password
	/// </summary>
	InvalidEmailOrPassword,
	/// <summary>
	/// Invalid email verification token
	/// </summary>
	InvalidEmailToken,
	/// <summary>
	/// Invalid file field name
	/// </summary>
	InvalidFileFieldName,
	/// <summary>
	/// Invalid format
	/// </summary>
	InvalidFormat,
	/// <summary>
	/// Invalid image data
	/// </summary>
	InvalidImageData,
	/// <summary>
	/// Invalid image format
	/// </summary>
	InvalidImageFormat,
	/// <summary>
	/// Invalid integer format
	/// </summary>
	InvalidIntegerFormat,
	/// <summary>
	/// Invalid snowflake format
	/// </summary>
	InvalidSnowflakeFormat,
	/// <summary>
	/// Invalid ISO timestamp
	/// </summary>
	InvalidIsoTimestamp,
	/// <summary>
	/// Invalid job ID
	/// </summary>
	InvalidJobId,
	/// <summary>
	/// Invalid JSON in payload_json field
	/// </summary>
	InvalidJsonInPayloadJson,
	/// <summary>
	/// Invalid message data
	/// </summary>
	InvalidMessageData,
	/// <summary>
	/// Invalid MFA code
	/// </summary>
	InvalidMfaCode,
	/// <summary>
	/// Invalid or expired authorisation ticket
	/// </summary>
	InvalidOrExpiredAuthorizationTicket,
	/// <summary>
	/// Invalid or expired authorisation token
	/// </summary>
	InvalidOrExpiredAuthorizationToken,
	/// <summary>
	/// Invalid or expired password reset token
	/// </summary>
	InvalidOrExpiredResetToken,
	/// <summary>
	/// Invalid or expired revert token
	/// </summary>
	InvalidOrExpiredRevertToken,
	/// <summary>
	/// Invalid or expired ticket
	/// </summary>
	InvalidOrExpiredTicket,
	/// <summary>
	/// Invalid or expired verification token
	/// </summary>
	InvalidOrExpiredVerificationToken,
	/// <summary>
	/// Invalid or restricted RTC region
	/// </summary>
	InvalidOrRestrictedRtcRegion,
	/// <summary>
	/// Invalid parent channel
	/// </summary>
	InvalidParentChannel,
	/// <summary>
	/// Invalid password
	/// </summary>
	InvalidPassword,
	/// <summary>
	/// Invalid proof token
	/// </summary>
	InvalidProofToken,
	/// <summary>
	/// Invalid role ID
	/// </summary>
	InvalidRoleId,
	/// <summary>
	/// Invalid RTC region
	/// </summary>
	InvalidRtcRegion,
	/// <summary>
	/// Invalid scheduled message payload
	/// </summary>
	InvalidScheduledMessagePayload,
	/// <summary>
	/// Invalid snowflake
	/// </summary>
	InvalidSnowflake,
	/// <summary>
	/// Invalid timeout value
	/// </summary>
	InvalidTimeoutValue,
	/// <summary>
	/// Invalid timezone identifier
	/// </summary>
	InvalidTimezoneIdentifier,
	/// <summary>
	/// Invalid URL format
	/// </summary>
	InvalidUrlFormat,
	/// <summary>
	/// Invalid URL or attachment format
	/// </summary>
	InvalidUrlOrAttachmentFormat,
	/// <summary>
	/// Invalid verification code
	/// </summary>
	InvalidVerificationCode,
	/// <summary>
	/// Invite splash requires the feature to be enabled
	/// </summary>
	InviteSplashRequiresFeature,
	/// <summary>
	/// Job ID is required
	/// </summary>
	JobIdIsRequired,
	/// <summary>
	/// Job has already been processed
	/// </summary>
	JobIsAlreadyProcessed,
	/// <summary>
	/// Job was not found
	/// </summary>
	JobNotFound,
	/// <summary>
	/// Media is already in favourite memes
	/// </summary>
	MediaAlreadyInFavoriteMemes,
	/// <summary>
	/// Message history cutoff cannot be before the guild was created
	/// </summary>
	MessageHistoryCutoffBeforeGuildCreation,
	/// <summary>
	/// Message history cutoff cannot be in the future
	/// </summary>
	MessageHistoryCutoffInFuture,
	/// <summary>
	/// Message IDs cannot be empty
	/// </summary>
	MessageIdsCannotBeEmpty,
	/// <summary>
	/// Messages array is required and cannot be empty
	/// </summary>
	MessagesArrayRequiredAndNotEmpty,
	/// <summary>
	/// Messages with snapshots cannot be edited
	/// </summary>
	MessagesWithSnapshotsCannotBeEdited,
	/// <summary>
	/// Total attachment size exceeds the maximum allowed
	/// </summary>
	MessageTotalAttachmentSizeTooLarge,
	/// <summary>
	/// Multiple files for the same index are not allowed
	/// </summary>
	MultipleFilesForIndexNotAllowed,
	/// <summary>
	/// Must agree to terms of service and privacy policy
	/// </summary>
	MustAgreeToTosAndPrivacyPolicy,
	/// <summary>
	/// Must be minimum age to use this service
	/// </summary>
	MustBeMinimumAge,
	/// <summary>
	/// Must enable 2FA before requiring it for moderators
	/// </summary>
	MustEnable2FaBeforeRequiringForMods,
	/// <summary>
	/// Must have an email to change it
	/// </summary>
	MustHaveEmailToChangeIt,
	/// <summary>
	/// Must start session before sending
	/// </summary>
	MustStartSessionBeforeSending,
	/// <summary>
	/// Name is empty after normalisation
	/// </summary>
	NameEmptyAfterNormalization,
	/// <summary>
	/// New email must be different from current email
	/// </summary>
	NewEmailMustBeDifferent,
	/// <summary>
	/// No file provided for attachment metadata
	/// </summary>
	NoFileForAttachmentMetadata,
	/// <summary>
	/// No file provided for attachment
	/// </summary>
	NoFileForAttachment,
	/// <summary>
	/// No metadata provided for file
	/// </summary>
	NoMetadataForFile,
	/// <summary>
	/// No new email has been requested
	/// </summary>
	NoNewEmailRequested,
	/// <summary>
	/// No original email on record
	/// </summary>
	NoOriginalEmailOnRecord,
	/// <summary>
	/// No valid media in message
	/// </summary>
	NoValidMediaInMessage,
	/// <summary>
	/// Not a valid Unicode emoji
	/// </summary>
	NotAValidUnicodeEmoji,
	/// <summary>
	/// Original email is already verified
	/// </summary>
	OriginalEmailAlreadyVerified,
	/// <summary>
	/// Original email must be verified first
	/// </summary>
	OriginalEmailMustBeVerifiedFirst,
	/// <summary>
	/// Original verification is not required
	/// </summary>
	OriginalVerificationNotRequired,
	/// <summary>
	/// Parent channel is not in the guild
	/// </summary>
	ParentChannelNotInGuild,
	/// <summary>
	/// Parent channel must be a category
	/// </summary>
	ParentMustBeCategory,
	/// <summary>
	/// Parse and users/roles cannot be used together
	/// </summary>
	ParseAndUsersOrRolesCannotBeUsedTogether,
	/// <summary>
	/// Password is too common
	/// </summary>
	PasswordIsTooCommon,
	/// <summary>
	/// Password length is invalid
	/// </summary>
	PasswordLengthInvalid,
	/// <summary>
	/// Password is not set
	/// </summary>
	PasswordNotSet,
	/// <summary>
	/// payload_json is required for multipart requests
	/// </summary>
	PayloadJsonRequiredForMultipart,
	/// <summary>
	/// Phone number has an invalid format
	/// </summary>
	PhoneNumberInvalidFormat,
	/// <summary>
	/// Preceding channel must share the same parent
	/// </summary>
	PrecedingChannelMustShareParent,
	/// <summary>
	/// Preceding channel is not in the guild
	/// </summary>
	PrecedingChannelNotInGuild,
	/// <summary>
	/// Preceding role is not in the guild
	/// </summary>
	PrecedingRoleNotInGuild,
	/// <summary>
	/// Premium is required for custom emoji
	/// </summary>
	PremiumRequiredForCustomEmoji,
	/// <summary>
	/// Pronouns have been changed too many times recently
	/// </summary>
	PronounsChangedTooManyTimes,
	/// <summary>
	/// Recipient IDs cannot be empty
	/// </summary>
	RecipientIdsCannotBeEmpty,
	/// <summary>
	/// Recipient IDs must be strings
	/// </summary>
	RecipientIdsMustBeStrings,
	/// <summary>
	/// Recipient IDs must be valid snowflakes
	/// </summary>
	RecipientIdsMustBeValidSnowflakes,
	/// <summary>
	/// Referenced attachment was not found
	/// </summary>
	ReferencedAttachmentNotFound,
	/// <summary>
	/// Rows field is required
	/// </summary>
	RowsIsRequired,
	/// <summary>
	/// Scheduled messages must be within 30 days
	/// </summary>
	ScheduledMessagesMax30Days,
	/// <summary>
	/// Scheduled time must be in the future
	/// </summary>
	ScheduledTimeMustBeFuture,
	/// <summary>
	/// Session has timed out
	/// </summary>
	SessionTimeout,
	/// <summary>
	/// Size in bytes must be a valid integer
	/// </summary>
	SizeBytesMustBeValidInteger,
	/// <summary>
	/// String must be exactly the required length
	/// </summary>
	StringLengthExact,
	/// <summary>
	/// String length is invalid
	/// </summary>
	StringLengthInvalid,
	/// <summary>
	/// System channel must be in the guild
	/// </summary>
	SystemChannelMustBeInGuild,
	/// <summary>
	/// System channel must be a text channel
	/// </summary>
	SystemChannelMustBeText,
	/// <summary>
	/// Tag is already taken
	/// </summary>
	TagAlreadyTaken,
	/// <summary>
	/// This vanity URL is already taken
	/// </summary>
	ThisVanityUrlIsAlreadyTaken,
	/// <summary>
	/// Ticket has already been completed
	/// </summary>
	TicketAlreadyCompleted,
	/// <summary>
	/// Timeout cannot exceed 365 days
	/// </summary>
	TimeoutCannotExceed365Days,
	/// <summary>
	/// Too many embeds
	/// </summary>
	TooManyEmbeds,
	/// <summary>
	/// Too many files
	/// </summary>
	TooManyFiles,
	/// <summary>
	/// Too many users with this username
	/// </summary>
	TooManyUsersWithThisUsername,
	/// <summary>
	/// Too many users with this username, try a different one
	/// </summary>
	TooManyUsersWithUsernameTryDifferent,
	/// <summary>
	/// Unclaimed accounts can only set email via verification token
	/// </summary>
	UnclaimedAccountsCanOnlySetEmailViaToken,
	/// <summary>
	/// Unknown image format
	/// </summary>
	UnknownImageFormat,
	/// <summary>
	/// Unresolved attachment URL
	/// </summary>
	UnresolvedAttachmentUrl,
	/// <summary>
	/// Uploaded attachment was not found
	/// </summary>
	UploadedAttachmentNotFound,
	/// <summary>
	/// URL length is invalid
	/// </summary>
	UrlLengthInvalid,
	/// <summary>
	/// User does not have an email address
	/// </summary>
	UserDoesNotHaveAnEmailAddress,
	/// <summary>
	/// User is not banned
	/// </summary>
	UserIsNotBanned,
	/// <summary>
	/// User must be a bot to be marked as a system user
	/// </summary>
	UserMustBeABotToBeMarkedAsASystemUser,
	/// <summary>
	/// User is not in the channel
	/// </summary>
	UserNotInChannel,
	/// <summary>
	/// Username cannot contain reserved terms
	/// </summary>
	UsernameCannotContainReservedTerms,
	/// <summary>
	/// Username has been changed too many times recently
	/// </summary>
	UsernameChangedTooManyTimes,
	/// <summary>
	/// Username contains invalid characters
	/// </summary>
	UsernameInvalidCharacters,
	/// <summary>
	/// Username length is invalid
	/// </summary>
	UsernameLengthInvalid,
	/// <summary>
	/// Username is a reserved value
	/// </summary>
	UsernameReservedValue,
	/// <summary>
	/// Value must be an integer in the valid range
	/// </summary>
	ValueMustBeIntegerInRange,
	/// <summary>
	/// Value is too small
	/// </summary>
	ValueTooSmall,
	/// <summary>
	/// Vanity URL code is already taken
	/// </summary>
	VanityUrlCodeAlreadyTaken,
	/// <summary>
	/// Vanity URL code cannot contain fluxer
	/// </summary>
	VanityUrlCodeCannotContainFluxer,
	/// <summary>
	/// Vanity URL code length is invalid
	/// </summary>
	VanityUrlCodeLengthInvalid,
	/// <summary>
	/// Vanity URL contains invalid characters
	/// </summary>
	VanityUrlInvalidCharacters,
	/// <summary>
	/// Vanity URL requires the feature to be enabled
	/// </summary>
	VanityUrlRequiresFeature,
	/// <summary>
	/// Verification code has expired
	/// </summary>
	VerificationCodeExpired,
	/// <summary>
	/// Verification code was not issued
	/// </summary>
	VerificationCodeNotIssued,
	/// <summary>
	/// Visionary subscription required for bot discriminator
	/// </summary>
	VisionaryRequiredForBotDiscriminator,
	/// <summary>
	/// Visionary subscription required for discriminator
	/// </summary>
	VisionaryRequiredForDiscriminator,
	/// <summary>
	/// Bot discriminator cannot be changed
	/// </summary>
	BotDiscriminatorCannotBeChanged,
	/// <summary>
	/// Voice channels cannot be positioned above text channels
	/// </summary>
	VoiceChannelsCannotBeAboveTextChannels,
	/// <summary>
	/// Webhook name length is invalid
	/// </summary>
	WebhookNameLengthInvalid,
}