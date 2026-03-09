using System.Text.Json.Serialization;
using Fluxify.Core;
using Fluxify.Dto.Channels.Category;
using Fluxify.Dto.Channels.LinkChannel;
using Fluxify.Dto.Channels.Text;
using Fluxify.Dto.Channels.Voice;

namespace Fluxify.Dto.Channels;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(ChannelCreateCategoryRequest), (int)ChannelType.Category)]
[JsonDerivedType(typeof(ChannelCreateTextRequest), (int)ChannelType.TextChannel)]
[JsonDerivedType(typeof(ChannelCreateLinkRequest), (int)ChannelType.LinkChannel)]
[JsonDerivedType(typeof(ChannelCreateVoiceRequest), (int)ChannelType.VoiceChannel)]
public abstract record ChannelCreateRequest(
    string Name,
    Snowflake? ParentId,
    ChannelPermissionOverwrite[]? PermissionOverwrites
);