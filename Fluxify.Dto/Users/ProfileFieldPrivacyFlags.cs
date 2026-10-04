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
/// Defines the privacy settings for a profile field.
/// </summary>
[Flags]
public enum ProfileFieldPrivacyFlags
{
    /// <summary>
    /// Everyone can see the profile field
    /// </summary>
    Everyone = 1 << 0,
    /// <summary>
    /// Only friends can see the profile field
    /// </summary>
    Friends = 1 << 1,
    /// <summary>
    /// Only users that share a guild with the user can see the profile field
    /// </summary>
    MutualGuilds = 1 << 2,
}