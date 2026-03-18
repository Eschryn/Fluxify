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

using System.Collections.Frozen;
using Fluxify.Core.Events;
using Fluxify.Core.Types;
using Fluxify.Dto.Channels;
using Fluxify.Dto.Channels.Text.Messages;
using Fluxify.Dto.Guilds;
using Fluxify.Dto.Guilds.AuditLog;
using Fluxify.Dto.Guilds.Members;
using Fluxify.Dto.SavedMedia;
using Fluxify.Dto.Users;
using Fluxify.Dto.Users.GuildSettings;
using Fluxify.Dto.Users.Relationships;
using Fluxify.Dto.Users.Settings;
using Fluxify.Gateway.Model.Data;
using Fluxify.Gateway.Model.Data.Channel;
using Fluxify.Gateway.Model.Data.Channel.Message;
using Fluxify.Gateway.Model.Data.Channel.Reaction;
using Fluxify.Gateway.Model.Data.Guild;
using Fluxify.Gateway.Model.Data.Guild.Roles;
using Fluxify.Gateway.Model.Data.User;
using Fluxify.Gateway.Model.Data.Voice;

namespace Fluxify.Gateway.Model;

internal static class EventNamePayloadClassMap
{
    internal static readonly FrozenDictionary<string, Type> TypeTable;
    internal static readonly FrozenDictionary<string, Func<IHandlerContainer>> HandlerContainerConstructorTable;

    static EventNamePayloadClassMap()
    {
        var sourceTable = new Dictionary<string, (Type, Func<IHandlerContainer>)>
        {
            { GatewayEvent.Ready, Data<ReadyPayload>() },
            { GatewayEvent.Resumed, Null },
            { GatewayEvent.SessionsReplace, Data<GatewaySession[]>() },

            // new all need to be tested
            { GatewayEvent.UserUpdate, Data<UserPrivate>() }, // untested
            { GatewayEvent.UserPinnedDmsUpdate, Data<Snowflake[]>() }, // untested
            { GatewayEvent.UserSettingsUpdate, Data<UserSettings>() }, // untested
            { GatewayEvent.UserGuildSettingsUpdate, Data<UserGuildSettingsResponse>() }, // untested
            { GatewayEvent.UserNoteUpdate, Data<GatewayUserNoteUpdate>() }, // untested
            { GatewayEvent.RecentMentionDelete, Data<GatewayMessageIdResponse>() }, // untested
            { GatewayEvent.SavedMessageCreate, Data<MessageResponse>() }, // untested
            { GatewayEvent.SavedMessageDelete, Data<GatewayMessageIdResponse>() }, // untested

            { GatewayEvent.FavoriteMemeCreate, Data<FavoriteMemeResponse>() }, // untested
            { GatewayEvent.FavoriteMemeUpdate, Data<FavoriteMemeResponse>() }, // untested
            { GatewayEvent.FavoriteMemeDelete, Data<GatewayMemeIdResponse>() }, // untested

            { GatewayEvent.AuthSessionChange, Data<GatewayAuthSessionChange>() }, // untested
            { GatewayEvent.PresenceUpdate, Data<Presence>() }, // untested

            { GatewayEvent.GuildCreate, Data<GatewayGuildCreate>() }, // untested
            { GatewayEvent.GuildUpdate, Data<GuildResponse>() }, // untested
            { GatewayEvent.GuildDelete, Data<GatewayGuildDelete>() }, // untested

            { GatewayEvent.GuildMemberAdd, Data<GuildMemberResponse>() }, // untested
            { GatewayEvent.GuildMemberUpdate, Data<GuildMemberResponse>() }, // untested
            { GatewayEvent.GuildMemberRemove, Data<GatewayGuildMemberDelete>() }, // untested

            { GatewayEvent.GuildRoleCreate, Data<GatewayGuildRole>() }, // untested
            { GatewayEvent.GuildRoleUpdate, Data<GatewayGuildRole>() }, // untested
            { GatewayEvent.GuildRoleDelete, Data<GatewayGuildRoleDelete>() }, // untested
            { GatewayEvent.GuildRoleUpdateBulk, Data<GatewayGuildRoleBulk>() }, // untested

            { GatewayEvent.GuildEmojisUpdate, Data<GatewayEmojiUpdate>() }, // untested
            { GatewayEvent.GuildStickersUpdate, Data<GatewayStickerUpdate>() }, // untested

            { GatewayEvent.GuildBanAdd, Data<GatewayBanData>() }, // untested
            { GatewayEvent.GuildBanRemove, Data<GatewayBanData>() }, // untested

            { GatewayEvent.ChannelCreate, Data<ChannelResponse>() }, // untested
            { GatewayEvent.ChannelUpdate, Data<ChannelResponse>() }, // untested
            { GatewayEvent.ChannelUpdateBulk, Data<GatewayBulkChannelUpdate>() }, // untested
            { GatewayEvent.ChannelDelete, Data<ChannelResponse>() }, // untested
            { GatewayEvent.ChannelPinsUpdate, Data<GatewayChannelPinsUpdate>() }, // untested
            { GatewayEvent.ChannelPinsAck, Data<GatewayChannelPinsAck>() }, // untested
            { GatewayEvent.ChannelRecipientAdd, Data<GatewayGroupChange>() }, // untested
            { GatewayEvent.ChannelRecipientRemove, Data<GatewayGroupChange>() }, // untested

            { GatewayEvent.MessageCreate, Data<GatewayMessageCreate>() },
            { GatewayEvent.MessageUpdate, Data<MessageResponse>() },
            { GatewayEvent.MessageDelete, Data<GatewayMessageDelete>() }, // untested
            { GatewayEvent.MessageDeleteBulk, Data<GatewayMessageDeleteBulk>() }, // untested
            { GatewayEvent.MessageReactionAdd, Data<GatewayReaction>() }, // untested
            { GatewayEvent.MessageReactionRemove, Data<GatewayReaction>() }, // untested
            { GatewayEvent.MessageReactionRemoveAll, Data<GatewayReactionRemoveAll>() }, // untested
            { GatewayEvent.MessageReactionRemoveEmoji, Data<GatewayReactionRemoveEmoji>() }, // untested
            { GatewayEvent.MessageAck, Data<GatewayMessageAck>() }, // untested

            { GatewayEvent.TypingStart, Data<GatewayTypingStart>() }, // untested

            { GatewayEvent.WebhooksUpdate, Data<GatewayChannelId>() }, // untested

            // { GatewayEvent.InviteCreate, Null }, // untested TODO: Add proper payload
            { GatewayEvent.InviteDelete, Data<GatewayInviteDelete>() }, // untested

            { GatewayEvent.RelationshipAdd, Data<RelationshipResponse>() }, // untested
            { GatewayEvent.RelationshipUpdate, Data<RelationshipResponse>() }, // untested
            { GatewayEvent.RelationshipRemove, Data<GatewayRelationshipId>() }, // untested

            { GatewayEvent.VoiceStateUpdate, Data<VoiceStateResponse>() }, // untested
            { GatewayEvent.VoiceServerUpdate, Data<GatewayVoiceServer>() }, // untested

            { GatewayEvent.CallCreate, Data<GatewayCallSchema>() }, // untested
            { GatewayEvent.CallUpdate, Data<GatewayCallSchema>() }, // untested
            { GatewayEvent.CallDelete, Data<GatewayChannelId>() }, // untested
            
            { GatewayEvent.GuildAuditLogEntryCreate, Data<GuildAuditLogEntryResponse>() } // untested
        };

        TypeTable = sourceTable.ToFrozenDictionary(k => k.Key, v => v.Value.Item1);
        HandlerContainerConstructorTable = sourceTable.ToFrozenDictionary(k => k.Key, v => v.Value.Item2);
    }

    private static readonly (Type, Func<IHandlerContainer>) Null = (typeof(object), () => new HandlerContainer());

    private static (Type, Func<IHandlerContainer>) Data<T>() => (typeof(T), () => new HandlerContainer<T>());
}