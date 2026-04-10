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

using System.Net.Mail;
using Fluxify.Core.Types;

namespace Fluxify.Application.Model.Messages;

public class Attachment
{
    public Snowflake Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public required string Filename { get; set; }
    public AttachmentFlags Flags { get; set; } = AttachmentFlags.None;
    public string? ContentHash { get; internal set; }
    public string? ContentType { get; internal set; }
    public int? Duration { get; internal set; }
    public bool? Expired { get; internal set; }
    public DateTimeOffset? ExpiresAt { get; internal set; }
    public int? Height { get; internal set; }   
    public bool? Nsfw { get; internal set; }
    public string? Placeholder { get; internal set; }   
    public string? ProxyUrl { get; internal set; }  
    public int Size { get; internal set; }
    public string? Url { get; internal set; }
    public string? Waveform { get; internal set; } 
    public int? Width { get; internal set; }
}

public class AttachmentProperties
{
    public Snowflake Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public required string Filename { get; set; }
    public AttachmentFlags Flags { get; set; } = AttachmentFlags.None;
} 