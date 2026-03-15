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

using System.ComponentModel.DataAnnotations;
using Fluxify.Core.Types;

namespace Fluxify.Dto.Channels.Text.Messages.Attachments;

/// <param name="Id">The attachment's ID must correspond to a file upload when creating a message.</param>
/// <param name="Title">The attachment's title.</param>
/// <param name="Description">The attachment's description.</param>
/// <param name="Filename">The attachment's filename.</param>
/// <param name="Flags">The attachment's flags.</param>
public sealed record MessageAttachmentRequest(
    Snowflake Id,
    [property: StringLength(1024)]
    string? Title,
    [property: StringLength(4096)]
    string? Description,
    [property: StringLength(255)]
    string Filename,
    MessageAttachmentFlags Flags
);