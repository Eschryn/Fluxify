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

namespace Fluxify.Dto.Users;

/// <summary>
/// Provides attributes that tell whether the user wants to be mentioned or not.
/// </summary>
public enum MentionFlags
{
    /// <summary>
    /// The user has stated no preference whether they should be mentioned or not.
    /// </summary>
    NoPreference = 0,

    /// <summary>
    /// The user prefers to be mentioned.
    /// </summary>
    PreferMention = 1,

    /// <summary>
    /// The user prefers not to be mentioned.
    /// </summary>
    PreferNoMention = 2
}