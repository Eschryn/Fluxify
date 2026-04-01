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
    string Name
);