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
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Fluxify.Core.Attributes;
using Fluxify.Dto.Channels;
using Fluxify.Dto.Channels.Text.Messages;
using Fluxify.Dto.Guilds.AuditLog;
using Fluxify.Dto.Guilds.Emoji;
using Fluxify.Dto.Guilds.Invite;
using Fluxify.Dto.Guilds.Members;
using Fluxify.Dto.Guilds.Roles;
using Fluxify.Dto.Json;
using Fluxify.Dto.Users.GuildSettings;
using Fluxify.Dto.Webhooks;

namespace Fluxify.Application.Model.AuditLog;

[Mapper]
internal sealed partial class AuditLogMapper(FluxerApplication app)
{
    private CacheMapper CacheMapper { get; } = app.CacheMapper;

    private static readonly FrozenDictionary<Type, PropertyTypeResolver> TypeResolvers =
        BuildTypeResolvers();

    private static readonly FrozenDictionary<Type, Func<AuditLogChangeSchema, IAuditLogChange>> ChangeConstructorMap
        = new Dictionary<Type, Func<AuditLogChangeSchema, IAuditLogChange>>
        {
            { typeof(bool), arg => MapChange(arg, e => e?.GetBoolean()) },
            { typeof(char), arg => MapChange(arg, e => e?.GetString()?.FirstOrDefault()) },
            { typeof(byte), arg => MapChange(arg, Extensions.GetByteWebsafe) },
            { typeof(sbyte), arg => MapChange(arg, Extensions.GetSByteWebsafe) },
            { typeof(short), arg => MapChange(arg, Extensions.GetInt16Websafe) },
            { typeof(ushort), arg => MapChange(arg, Extensions.GetUInt16Websafe) },
            { typeof(int), arg => MapChange(arg, Extensions.GetInt32Websafe) },
            { typeof(uint), arg => MapChange(arg, Extensions.GetUInt32Websafe) },
            { typeof(long), arg => MapChange(arg, Extensions.GetInt64Websafe) },
            { typeof(ulong), arg => MapChange(arg, Extensions.GetUInt64Websafe) },
            { typeof(float), arg => MapChange(arg, Extensions.GetSingleWebsafe) },
            { typeof(double), arg => MapChange(arg, Extensions.GetDoubleWebsafe) },
            { typeof(decimal), arg => MapChange(arg, Extensions.GetDecimalWebsafe) },
            { typeof(string), arg => MapChange(arg, e => e?.GetString()) },
            { typeof(Guid), arg => MapChange(arg, e => e?.GetGuid()) },
            { typeof(DateTime), arg => MapChange(arg, e => e?.GetDateTime()) },
            { typeof(DateTimeOffset), arg => MapChange(arg, e => e?.GetDateTimeOffset()) },
            { typeof(Snowflake), arg => MapChange(arg, e => e?.GetSnowflake()) },
            { typeof(Permissions), arg => MapChange(arg, e => (Permissions?)e.GetUInt64Websafe()) },
        }.ToFrozenDictionary();

    private static readonly FrozenDictionary<Type, Func<AuditLogChangeSchema, IAuditLogChange>>
        ChangeArrayConstructorMap
            = new Dictionary<Type, Func<AuditLogChangeSchema, IAuditLogChange>>
            {
                { typeof(bool), arg => MapArrayChange(arg, e => e?.GetBoolean()) },
                { typeof(char), arg => MapArrayChange(arg, e => e?.GetString()?.FirstOrDefault()) },
                { typeof(byte), arg => MapArrayChange(arg, Extensions.GetByteWebsafe) },
                { typeof(sbyte), arg => MapArrayChange(arg, Extensions.GetSByteWebsafe) },
                { typeof(short), arg => MapArrayChange(arg, Extensions.GetInt16Websafe) },
                { typeof(ushort), arg => MapArrayChange(arg, Extensions.GetUInt16Websafe) },
                { typeof(int), arg => MapArrayChange(arg, Extensions.GetInt32Websafe) },
                { typeof(uint), arg => MapArrayChange(arg, Extensions.GetUInt32Websafe) },
                { typeof(long), arg => MapArrayChange(arg, Extensions.GetInt64Websafe) },
                { typeof(ulong), arg => MapArrayChange(arg, Extensions.GetUInt64Websafe) },
                { typeof(float), arg => MapArrayChange(arg, Extensions.GetSingleWebsafe) },
                { typeof(double), arg => MapArrayChange(arg, Extensions.GetDoubleWebsafe) },
                { typeof(decimal), arg => MapArrayChange(arg, Extensions.GetDecimalWebsafe) },
                { typeof(string), arg => MapArrayChange(arg, e => e?.GetString()) },
                { typeof(Guid), arg => MapArrayChange(arg, e => e?.GetGuid()) },
                { typeof(DateTime), arg => MapArrayChange(arg, e => e?.GetDateTime()) },
                { typeof(DateTimeOffset), arg => MapArrayChange(arg, e => e?.GetDateTimeOffset()) },
                { typeof(Snowflake), arg => MapArrayChange(arg, e => e?.GetSnowflake()) },
                { typeof(Permissions), arg => MapArrayChange(arg, e => (Permissions?)e.GetUInt64Websafe()) },
            }.ToFrozenDictionary();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static IAuditLogChange<T> MapChange<T>(AuditLogChangeSchema auditLogChangeSchema,
        Func<JsonElement?, T?> jsonConverter)
        => new AuditLogChange<T>(
            auditLogChangeSchema.Key,
            jsonConverter(auditLogChangeSchema.OldValue),
            jsonConverter(auditLogChangeSchema.NewValue)
        );

    private static IAuditLogChange<T?[]> MapArrayChange<T>(AuditLogChangeSchema auditLogChangeSchema,
        Func<JsonElement?, T?> jsonConverter)
        => new AuditLogChange<T?[]>(
            auditLogChangeSchema.Key,
            auditLogChangeSchema.OldValue
                ?.EnumerateArray()
                .Select(e => e.ValueKind == JsonValueKind.Null ? null : (JsonElement?)e)
                .Select(jsonConverter)
                .ToArray(),
            auditLogChangeSchema.NewValue
                ?.EnumerateArray()
                .Select(e => e.ValueKind == JsonValueKind.Null ? null : (JsonElement?)e)
                .Select(jsonConverter)
                .ToArray()
        );

    private static readonly FrozenDictionary<AuditLogActionType, PropertyTypeResolver> ActionTypeResolvers =
        new Dictionary<AuditLogActionType, PropertyTypeResolver>
        {
            // 0-9: Guild Settings
            { AuditLogActionType.GuildSettingsUpdate, TypeResolvers[typeof(UserGuildSettingsResponse)] },
            // 10-19: Channels
            { AuditLogActionType.ChannelCreated, TypeResolvers[typeof(ChannelResponse)] },
            { AuditLogActionType.ChannelUpdated, TypeResolvers[typeof(ChannelResponse)] },
            { AuditLogActionType.ChannelDeleted, TypeResolvers[typeof(ChannelResponse)] },
            { AuditLogActionType.PermissionOverwriteCreated, TypeResolvers[typeof(ChannelPermissionOverwrite)] },
            { AuditLogActionType.PermissionOverwriteUpdated, TypeResolvers[typeof(ChannelPermissionOverwrite)] },
            { AuditLogActionType.PermissionOverwriteDeleted, TypeResolvers[typeof(ChannelPermissionOverwrite)] },
            // 20-29: Guild Members
            { AuditLogActionType.MemberKicked, TypeResolvers[typeof(GuildMemberResponse)] },
            { AuditLogActionType.MembersPruned, TypeResolvers[typeof(GuildMemberResponse)] },
            { AuditLogActionType.MemberBanned, TypeResolvers[typeof(GuildMemberResponse)] },
            { AuditLogActionType.MemberUnbanned, TypeResolvers[typeof(GuildMemberResponse)] },
            { AuditLogActionType.MemberUpdated, TypeResolvers[typeof(GuildMemberResponse)] },
            { AuditLogActionType.MemberRolesUpdated, TypeResolvers[typeof(GuildMemberResponse)] },
            { AuditLogActionType.VoiceMemberMoved, TypeResolvers[typeof(GuildMemberResponse)] },
            { AuditLogActionType.VoiceMemberDisconnected, TypeResolvers[typeof(GuildMemberResponse)] },
            // 30-39: Roles
            { AuditLogActionType.RoleCreated, TypeResolvers[typeof(GuildRoleResponse)] },
            { AuditLogActionType.RoleUpdated, TypeResolvers[typeof(GuildRoleResponse)] },
            { AuditLogActionType.RoleDeleted, TypeResolvers[typeof(GuildRoleResponse)] },
            // 40-49: Invites
            { AuditLogActionType.InviteCreated, TypeResolvers[typeof(GuildInviteResponse)] },
            { AuditLogActionType.InviteUpdated, TypeResolvers[typeof(GuildInviteResponse)] },
            { AuditLogActionType.InviteDeleted, TypeResolvers[typeof(GuildInviteResponse)] },
            // 50-59: Webhooks
            { AuditLogActionType.WebhookCreated, TypeResolvers[typeof(WebhookResponse)] },
            { AuditLogActionType.WebhookUpdated, TypeResolvers[typeof(WebhookResponse)] },
            { AuditLogActionType.WebhookDeleted, TypeResolvers[typeof(WebhookResponse)] },
            // 60-69: Emojis
            { AuditLogActionType.EmojiCreated, TypeResolvers[typeof(GuildEmojiResponse)] },
            { AuditLogActionType.EmojiUpdated, TypeResolvers[typeof(GuildEmojiResponse)] },
            { AuditLogActionType.EmojiDeleted, TypeResolvers[typeof(GuildEmojiResponse)] },
            // 70-79: Messages
            { AuditLogActionType.MessageDeleted, TypeResolvers[typeof(MessageResponse)] },
            { AuditLogActionType.MessagesBulkDeleted, TypeResolvers[typeof(MessageResponse)] },
            { AuditLogActionType.MessagePinned, TypeResolvers[typeof(MessageResponse)] },
            { AuditLogActionType.MessageUnpinned, TypeResolvers[typeof(MessageResponse)] },
            // 80-89: --

            // 90-99: Stickers
            { AuditLogActionType.StickerCreated, TypeResolvers[typeof(StickerResponse)] },
            { AuditLogActionType.StickerUpdated, TypeResolvers[typeof(StickerResponse)] },
            { AuditLogActionType.StickerDeleted, TypeResolvers[typeof(StickerResponse)] },
        }.ToFrozenDictionary();

    static FrozenDictionary<Type, PropertyTypeResolver> BuildTypeResolvers()
    {
        var auditLoggableTypes = new[]
        {
            typeof(ChannelResponse),
            typeof(WebhookResponse),
            typeof(MessageResponse),
            typeof(StickerResponse),
            typeof(GuildRoleResponse),
            typeof(GuildEmojiResponse),
            typeof(GuildMemberResponse),
            typeof(GuildInviteResponse),
            typeof(UserGuildSettingsResponse),
            typeof(ChannelPermissionOverwrite),
        };

        return new Dictionary<Type, PropertyTypeResolver>(
            auditLoggableTypes
                .Select(t => new KeyValuePair<Type, PropertyTypeResolver>(t, PropertyTypeResolver.Build(t)))
        ).ToFrozenDictionary();
    }

    private IAuditLogChange InferValueFromJson(AuditLogChangeSchema auditLogChangeSchema)
    {
        var valueKind = JsonValueKind.Undefined;
        var isInt = false;
        if (auditLogChangeSchema.NewValue is { ValueKind: not (JsonValueKind.Undefined or JsonValueKind.Null) } jNew)
        {
            valueKind = jNew.ValueKind;
            if (valueKind is JsonValueKind.Number)
            {
                isInt = jNew.TryGetInt64(out _);
            }
        }
        else if (auditLogChangeSchema.OldValue is
                 { ValueKind: not (JsonValueKind.Undefined or JsonValueKind.Null) } jOld)
        {
            valueKind = jOld.ValueKind;
            if (valueKind is JsonValueKind.Number)
            {
                isInt = jOld.TryGetInt64(out _);
            }
        }

        return valueKind switch
        {
            JsonValueKind.String => auditLogChangeSchema.ToStringChange(),
            JsonValueKind.True or JsonValueKind.False => auditLogChangeSchema.ToBooleanChange(),
            JsonValueKind.Number => isInt switch
            {
                true => new AuditLogChange<long?>(
                    auditLogChangeSchema.Key,
                    auditLogChangeSchema.OldValue?.GetInt64(),
                    auditLogChangeSchema.NewValue?.GetInt64()
                ),
                _ => new AuditLogChange<double?>(
                    auditLogChangeSchema.Key,
                    auditLogChangeSchema.OldValue?.GetDouble(),
                    auditLogChangeSchema.NewValue?.GetDouble()
                )
            },
            JsonValueKind.Array or JsonValueKind.Object => new AuditLogChange<JsonElement?>(
                auditLogChangeSchema.Key,
                auditLogChangeSchema.OldValue,
                auditLogChangeSchema.NewValue
            ),
            JsonValueKind.Null or JsonValueKind.Undefined => new AuditLogChange<object?>(
                auditLogChangeSchema.Key,
                null,
                null
            ),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public AuditLogEntry MapFromResponse(
        GuildAuditLogEntryResponse response
    )
    {
        ActionTypeResolvers.TryGetValue(response.ActionType, out var resolver);

        return new AuditLogEntry(
            response.Id,
            response.ActionType,
            response.Changes?.Select(c => MapChange(resolver, c)).ToArray(),
            response.Options,
            response.TargetId,
            response.Reason,
            response.UserId
        );
    }

    private IAuditLogChange MapChange(PropertyTypeResolver? resolver, AuditLogChangeSchema arg)
    {
        try
        {
            if (resolver?.Resolve(arg.Key) is { } type)
            {
                if (type.IsArray)
                {
                    var elementType = type.HasElementType ? type.GetElementType() : type.GenericTypeArguments[0];

                    if (elementType != null
                        && ChangeArrayConstructorMap.TryGetValue(elementType, out var constructor))
                        return constructor(arg);
                }
                else if (ChangeConstructorMap.TryGetValue(type, out var constructor))
                    return constructor(arg);
            }
        }
        catch (Exception)
        {
            // fall back to inferring from json
        }

        return InferValueFromJson(arg);
    }

    private object?[]? MapJsonArray(JsonElement element)
    {
        if (element.ValueKind != JsonValueKind.Array)
        {
            return null;
        }

        if (element.GetArrayLength() == 0)
        {
            return [];
        }

        return element.EnumerateArray()
            .Select(MapJsonArrayElement)
            .ToArray();
    }

    private object? MapJsonArrayElement(JsonElement element) =>
        element.ValueKind switch
        {
            JsonValueKind.Undefined => null,
            JsonValueKind.Object => element,
            JsonValueKind.Array => MapJsonArray(element),
            JsonValueKind.String => element.GetString(),
            JsonValueKind.Number => element.GetInt64(),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Null => null,
            _ => throw new ArgumentOutOfRangeException()
        };

    public AuditLogEntry[] MapFromResponse(GuildAuditLogListResponse response)
    {
        foreach (var userPartialResponse in response.Users)
        {
            CacheMapper.InsertGlobalUser(userPartialResponse);
        }

        return response.Entries.Select(MapFromResponse).ToArray();
    }
}

internal sealed partial class PropertyTypeResolver(FrozenDictionary<string, Type> fieldMappings)
{
    public Type? Resolve(string propertyName) => fieldMappings.GetValueOrDefault(propertyName);

    public static PropertyTypeResolver Build(Type type)
    {
        var dtoAssembly = typeof(DtoJsonContext).Assembly;
        if (type.Assembly != dtoAssembly)
        {
            throw new ArgumentException("Type must be from Dto package", nameof(type));
        }

        var availableProperties = dtoAssembly
            .DefinedTypes
            .Where(t => t.IsAssignableTo(type))
            .SelectMany(ti => (IEnumerable<AuditLogPropertyMeta>)
            [
                .. ti.DeclaredProperties.Select(pi => new AuditLogPropertyMeta(pi.Name, pi.PropertyType, ti.AsType())),
                
                // special audit log properties
                .. ti.GetCustomAttributes<AuditLogPropertyAttribute>()
                    .Select(attr => new AuditLogPropertyMeta(attr.Name, attr.Type, ti.AsType())),
                
                // type property on polymorphic types
                .. ti.GetCustomAttributes<JsonPolymorphicAttribute>()
                    .Select(attr => new AuditLogPropertyMeta(
                        attr.TypeDiscriminatorPropertyName ?? "$type",
                        ti.GetCustomAttributes<JsonDerivedTypeAttribute>()
                            .FirstOrDefault()
                            ?.TypeDiscriminator
                            ?.GetType()
                            ?? typeof(string),
                        ti.AsType()
                    ))
            ])
            .DistinctBy(p => p.Name)
            .ToDictionary(
                keySelector: PropertyNameSelector,
                elementSelector: p => Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType
            );

        return new PropertyTypeResolver(availableProperties.ToFrozenDictionary());
    }

    private record AuditLogPropertyMeta(string Name, Type PropertyType, Type DeclaringType);

    private static string PropertyNameSelector(AuditLogPropertyMeta p)
    {
        // Special case:
        // in the fluxer data model (what gets stored in the database),
        // the id property is named "<entity>_id"
        if (p.Name == "Id")
        {
            var typeName = p.DeclaringType.Name;

            return PascalCaseToCamelCase(typeName.EndsWith("Response") ? typeName[..^8] : typeName) + "_id";
        }

        return PascalCaseToCamelCase(p.Name);
    }

    [GeneratedRegex("[A-Z]")]
#if NET9_0_OR_GREATER
    private static partial Regex PascalCaseRegex { get; }
#else
    private static partial Regex PascalCaseRegex();
#endif

    private static string PascalCaseToCamelCase(string name)
#if NET9_0_OR_GREATER
        => PascalCaseRegex
#else
        => PascalCaseRegex()
#endif
            .Replace(name, m => char.IsUpper(m.Value[0]) ? "_" + char.ToLowerInvariant(m.Value[0]) : m.Value)
            .TrimStart('_');
}

file static class Extensions
{
    extension(AuditLogChangeSchema schema)
    {
        public AuditLogChange<bool?> ToBooleanChange() =>
            new(schema.Key, schema.OldValue?.GetBoolean(), schema.NewValue?.GetBoolean());

        public AuditLogChange<string> ToStringChange() =>
            new(schema.Key, schema.OldValue?.GetString(), schema.NewValue?.GetString());
    }

    extension(JsonElement schema)
    {
        public Snowflake GetSnowflake()
        {
            if (schema.ValueKind == JsonValueKind.String)
            {
                if (ulong.TryParse(schema.GetString(), out var result))
                {
                    return new Snowflake(result);
                }

                throw new JsonException("string could not be parsed as snowflake.");
            }
            else if (schema.ValueKind == JsonValueKind.Number)
                return new Snowflake(schema.GetUInt64());

            throw new JsonException();
        }
    }
    
    extension(JsonElement? e)
    {
        public byte? GetByteWebsafe() => e.HasValue
            ? e.Value.ValueKind == JsonValueKind.Number 
              && e.Value.TryGetByte(out var b) ? b : byte.Parse(e.Value.GetString()!)
            : null;
        
        public sbyte? GetSByteWebsafe() => e.HasValue
            ? e.Value.ValueKind == JsonValueKind.Number 
              && e.Value.TryGetSByte(out var b) ? b : sbyte.Parse(e.Value.GetString()!)
            : null;
        
        public short? GetInt16Websafe() => e.HasValue
            ? e.Value.ValueKind == JsonValueKind.Number 
              && e.Value.TryGetInt16(out var b) ? b : short.Parse(e.Value.GetString()!)
            : null;
        
        public ushort? GetUInt16Websafe() => e.HasValue
            ? e.Value.ValueKind == JsonValueKind.Number 
              && e.Value.TryGetUInt16(out var b) ? b : ushort.Parse(e.Value.GetString()!)
            : null;
        
        public int? GetInt32Websafe() => e.HasValue
            ? e.Value.ValueKind == JsonValueKind.Number 
              && e.Value.TryGetInt32(out var b) ? b : int.Parse(e.Value.GetString()!)
            : null;
        
        public uint? GetUInt32Websafe() => e.HasValue
            ? e.Value.ValueKind == JsonValueKind.Number 
              && e.Value.TryGetUInt32(out var b) ? b : uint.Parse(e.Value.GetString()!)
            : null;
        
        public long? GetInt64Websafe() => e.HasValue
            ? e.Value.ValueKind == JsonValueKind.Number 
              && e.Value.TryGetInt64(out var b) ? b : long.Parse(e.Value.GetString()!)
            : null;
        
        public ulong? GetUInt64Websafe() => e.HasValue
            ? e.Value.ValueKind == JsonValueKind.Number 
              && e.Value.TryGetUInt64(out var b) ? b : ulong.Parse(e.Value.GetString()!)
            : null;
        
        public float? GetSingleWebsafe() => e.HasValue
            ? e.Value.ValueKind == JsonValueKind.Number 
              && e.Value.TryGetSingle(out var b) ? b : float.Parse(e.Value.GetString()!)
            : null;
        
        public double? GetDoubleWebsafe() => e.HasValue
            ? e.Value.ValueKind == JsonValueKind.Number 
              && e.Value.TryGetDouble(out var b) ? b : double.Parse(e.Value.GetString()!)
            : null;
        
        public decimal? GetDecimalWebsafe() => e.HasValue
            ? e.Value.ValueKind == JsonValueKind.Number 
              && e.Value.TryGetDecimal(out var b) ? b : decimal.Parse(e.Value.GetString()!)
            : null;
    }
}