using System.Text.Json.Serialization;
using Fluxify.Dto.Channels.Category;
using Fluxify.Dto.Channels.GroupDm;
using Fluxify.Dto.Channels.LinkChannel;
using Fluxify.Dto.Channels.Text;
using Fluxify.Dto.Channels.Voice;

namespace Fluxify.Dto.Channels;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(ChannelUpdateVoiceRequest), (int)ChannelType.VoiceChannel)]
[JsonDerivedType(typeof(ChannelUpdateCategoryRequest), (int)ChannelType.Category)]
[JsonDerivedType(typeof(ChannelUpdateGroupDmRequest), (int)ChannelType.GroupDm)]
[JsonDerivedType(typeof(ChannelUpdateTextRequest), (int)ChannelType.TextChannel)]
[JsonDerivedType(typeof(ChannelUpdateLinkRequest), (int)ChannelType.LinkChannel)]
public abstract record ChannelUpdateRequest;