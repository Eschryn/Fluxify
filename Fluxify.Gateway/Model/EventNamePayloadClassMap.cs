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
using System.Text.Json.Serialization.Metadata;
using Fluxify.Core.Events;
using Fluxify.Core.Types;
using Fluxify.Dto.Channels;
using Fluxify.Dto.Channels.Text.Messages;
using Fluxify.Dto.Guilds;
using Fluxify.Dto.Guilds.AuditLog;
using Fluxify.Dto.Json;
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
    internal static readonly FrozenDictionary<string, JsonTypeInfo?> JsonTypeInfoTable;

    internal static readonly
        FrozenDictionary<string, (Func<IHandlerContainer> constructor, Func<IHandlerContainer, object, Task> invoker)>
        HandlerContainerConstructorTable;

    static EventNamePayloadClassMap()
    {
        var sourceTable =
            new Dictionary<string, (Type, (Func<IHandlerContainer> constructor, Func<IHandlerContainer, object, Task>
                invoker), JsonTypeInfo?)>
            {
                { GatewayEvent.Ready, Data<ReadyPayload>() },
                { GatewayEvent.Resumed, Null },
                { GatewayEvent.SessionsReplace, Data<GatewaySession[]>() },

                // new all need to be tested
                { GatewayEvent.UserUpdate, Data<UserPrivateReponse>() },
                { GatewayEvent.UserPinnedDmsUpdate, Data<Snowflake[]>() },
                { GatewayEvent.UserSettingsUpdate, Data<UserSettings>() },
                { GatewayEvent.UserGuildSettingsUpdate, Data<UserGuildSettingsResponse>() },
                { GatewayEvent.UserNoteUpdate, Data<GatewayUserNoteUpdate>() },
                { GatewayEvent.RecentMentionDelete, Data<GatewayMessageIdResponse>() },
                { GatewayEvent.SavedMessageCreate, Data<MessageResponse>() },
                { GatewayEvent.SavedMessageDelete, Data<GatewayMessageIdResponse>() },

                { GatewayEvent.FavoriteMemeCreate, Data<FavoriteMemeResponse>() },
                { GatewayEvent.FavoriteMemeUpdate, Data<FavoriteMemeResponse>() },
                { GatewayEvent.FavoriteMemeDelete, Data<GatewayMemeIdResponse>() },

                { GatewayEvent.AuthSessionChange, Data<GatewayAuthSessionChange>() },
                { GatewayEvent.PresenceUpdate, Data<PresenceResponse>() },

                { GatewayEvent.GuildCreate, Data<GatewayGuildCreate>() },
                { GatewayEvent.GuildUpdate, Data<GuildResponse>() },
                { GatewayEvent.GuildDelete, Data<GatewayGuildDelete>() },

                { GatewayEvent.GuildMemberAdd, Data<GatewayGuildMember>() },
                { GatewayEvent.GuildMemberUpdate, Data<GatewayGuildMember>() },
                { GatewayEvent.GuildMemberRemove, Data<GatewayGuildMemberDelete>() },

                { GatewayEvent.GuildRoleCreate, Data<GatewayGuildRole>() },
                { GatewayEvent.GuildRoleUpdate, Data<GatewayGuildRole>() },
                { GatewayEvent.GuildRoleDelete, Data<GatewayGuildRoleDelete>() },
                { GatewayEvent.GuildRoleUpdateBulk, Data<GatewayGuildRoleBulk>() },

                { GatewayEvent.GuildEmojisUpdate, Data<GatewayEmojiUpdate>() },
                { GatewayEvent.GuildStickersUpdate, Data<GatewayStickerUpdate>() },

                { GatewayEvent.GuildBanAdd, Data<GatewayBanData>() },
                { GatewayEvent.GuildBanRemove, Data<GatewayBanData>() },

                { GatewayEvent.ChannelCreate, Data<ChannelResponse>() },
                { GatewayEvent.ChannelUpdate, Data<ChannelResponse>() },
                { GatewayEvent.ChannelUpdateBulk, Data<GatewayBulkChannelUpdate>() },
                { GatewayEvent.ChannelDelete, Data<ChannelResponse>() },
                { GatewayEvent.ChannelPinsUpdate, Data<GatewayChannelPinsUpdate>() },
                { GatewayEvent.ChannelPinsAck, Data<GatewayChannelPinsAck>() },
                { GatewayEvent.ChannelRecipientAdd, Data<GatewayGroupChange>() },
                { GatewayEvent.ChannelRecipientRemove, Data<GatewayGroupChange>() },

                { GatewayEvent.MessageCreate, Data<GatewayMessage>() },
                { GatewayEvent.MessageUpdate, Data<GatewayMessage>() },
                { GatewayEvent.MessageDelete, Data<GatewayMessageDelete>() },
                { GatewayEvent.MessageDeleteBulk, Data<GatewayMessageDeleteBulk>() },
                { GatewayEvent.MessageReactionAdd, Data<GatewayReaction>() },
                { GatewayEvent.MessageReactionRemove, Data<GatewayReaction>() },
                { GatewayEvent.MessageReactionRemoveAll, Data<GatewayReactionRemoveAll>() },
                { GatewayEvent.MessageReactionRemoveEmoji, Data<GatewayReactionRemoveEmoji>() },
                { GatewayEvent.MessageAck, Data<GatewayMessageAck>() },

                { GatewayEvent.TypingStart, Data<GatewayTypingStart>() },

                { GatewayEvent.WebhooksUpdate, Data<GatewayChannelId>() },

                // { GatewayEvent.InviteCreate, Null }, TODO: Add proper payload
                { GatewayEvent.InviteDelete, Data<GatewayInviteDelete>() },

                { GatewayEvent.RelationshipAdd, Data<RelationshipResponse>() },
                { GatewayEvent.RelationshipUpdate, Data<RelationshipResponse>() },
                { GatewayEvent.RelationshipRemove, Data<GatewayRelationshipId>() },

                { GatewayEvent.VoiceStateUpdate, Data<VoiceStateResponse>() },
                { GatewayEvent.VoiceServerUpdate, Data<GatewayVoiceServer>() },

                { GatewayEvent.CallCreate, Data<GatewayCallSchema>() },
                { GatewayEvent.CallUpdate, Data<GatewayCallSchema>() },
                { GatewayEvent.CallDelete, Data<GatewayChannelId>() },

                { GatewayEvent.GuildAuditLogEntryCreate, Data<GuildAuditLogEntryResponse>() },

                { GatewayEvent.PassiveUpdates, Data<GatewayPassiveUpdate>() },
                { GatewayEvent.GuildCountsUpdate, Data<GuildMemberCountsUpdate>() }
            };

        TypeTable = sourceTable.ToFrozenDictionary(k => k.Key, v => v.Value.Item1);
        JsonTypeInfoTable = sourceTable.ToFrozenDictionary(k => k.Key, v => v.Value.Item3);
        HandlerContainerConstructorTable = sourceTable.ToFrozenDictionary(k => k.Key, v => v.Value.Item2);
    }

    private static readonly (Type, (Func<IHandlerContainer>, Func<IHandlerContainer, object, Task>), JsonTypeInfo?) Null
        = (typeof(object), (() => new HandlerContainer(), (h, _) => ((HandlerContainer)h).CallHandlersAsync()),
            GatewayJsonContext.Default.Object);

    private static (Type, (Func<IHandlerContainer>, Func<IHandlerContainer, object, Task>), JsonTypeInfo?) Data<T>()
        => (typeof(T), (() => new HandlerContainer<T>(), (h, o) => ((HandlerContainer<T>)h).CallHandlersAsync((T)o)),
            ResolveJsonTypeInfo<T>());

    private static JsonTypeInfo? ResolveJsonTypeInfo<T>()
    {
        var type = typeof(T);

        return GatewayJsonContext.Default.GetTypeInfo(type)
               ?? DtoJsonContext.Default.GetTypeInfo(type);
    }
}