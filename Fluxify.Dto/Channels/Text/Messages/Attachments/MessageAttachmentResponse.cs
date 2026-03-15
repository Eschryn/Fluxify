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

using Fluxify.Core.Types;

namespace Fluxify.Dto.Channels.Text.Messages.Attachments;

/// <summary>
/// 
/// </summary>
/// <param name="ContentHash"></param>
/// <param name="ContentType"></param>
/// <param name="Waveform">Base64 encoded waveform data</param>
/// <param name="Duration">Duration in seconds</param>
/// <param name="Placeholder">Base64 encoded placeholder image</param>
public record MessageAttachmentResponse(
    Snowflake Id,
    string Filename,
    string? Title,
    string? Description,
    string? ContentType,
    string? ContentHash,
    int Size,
    string? Url,
    string? ProxyUrl,
    int? Width,
    int? Height,
    string? Placeholder,
    MessageAttachmentFlags Flags,
    bool? Nsfw,
    int? Duration,
    string? Waveform,
    bool? Expired,
    DateTimeOffset? ExpiresAt
);