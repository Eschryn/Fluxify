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

using System.Drawing;
using Fluxify.Application.Entities.Guilds;
using Fluxify.Dto.Guilds.Emoji;
using Fluxify.Dto.Guilds.Stickers;
using Riok.Mapperly.Abstractions;

namespace Fluxify.Application;

[Mapper]
public static partial class CommonMapper 
{
    public static int MapToInt(Color color) => color.ToArgb() & 0x00FFFFFF;
    public static Color MapToColor(int color) => Color.FromArgb((int)((color & 0x00FFFFFF) | 0xFF000000));
    
    public static IEmoji MapToEmoji(GuildEmojiResponse e) => e.Id.HasValue ? MapToGuildEmoji(e) : MapToUnicodeEmoji(e);
    
    [MapProperty(nameof(GuildStickerResponse.Animated), nameof(Sticker.IsAnimated))]
    public static partial Sticker MapToSticker(GuildStickerResponse e);
    
    [MapperIgnoreSource(nameof(GuildStickerResponse.Id))]
    [MapperIgnoreSource(nameof(GuildStickerResponse.Animated))]
    public static partial void UpdateSticker([MappingTarget] Sticker target, GuildStickerResponse source);

    [MapProperty(nameof(GuildEmojiResponse.Animated), nameof(GuildEmoji.IsAnimated))]
    private static partial GuildEmoji MapToGuildEmoji(GuildEmojiResponse e);
    
    private static UnicodeEmoji MapToUnicodeEmoji(GuildEmojiResponse e) => new(e.Name);
}