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

using Fluxify.Application.Entities.Messages;
using Fluxify.Application.Model.Messages.Embeds;
using Fluxify.Core.Types;
using Fluxify.Dto.Uploads;

namespace Fluxify.Application.Model.Messages;

public class MessageDto
{
    public string? Content { get; set; }
    public List<Attachment>? Attachments { get; set; }
    public List<Embed>? Embeds { get; set; }
    public AllowedMentions? AllowedMentions { get; set; }
    public MessageReference? MessageReference { get; set; }
    public MessageFlags? Flags { get; set; }
    public string? Nonce { get; init; }
    public Snowflake? FavoriteMemeId { get; set; }
    public List<Snowflake>? Stickers { get; set; }
    public bool Tts { get; set; }
    public List<FileUpload>? Files { get; set; }
}