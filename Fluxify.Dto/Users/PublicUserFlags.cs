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
/// Provides known publicly visible attributes of a global user.
/// </summary>
[Flags]
public enum PublicUserFlags : uint
{
    /// <summary>
    /// The user is a staff member of the instance.
    /// </summary>
    Staff = 1 << 0,
    
    /*
     Has been removed
    /// <summary>
    /// Community team p???? member
    /// </summary>
    CtpMember = 1 << 1,
    */
    
    /// <summary>
    /// The user is participating in the partner program of the instance.
    /// </summary>
    Partner = 1 << 2,
    
    /// <summary>
    /// The user has found security related bugs (main fluxer instance) or for other instances it could also just mean generally the user found bugs.
    /// </summary>
    BugHunter = 1 << 3,
    
    /// <summary>
    /// The user is a bot account that accepts friend requests.
    /// </summary>
    FriendlyBot = 1 << 4,
    
    /// <summary>
    /// The user is a bot account that accepts friend requests after manual approval of the application owner.
    /// </summary>
    FriendlyBotManualApproval = 1 << 5,
    
    /// <summary>
    /// The user has been flagged as a spammer on the instance.
    /// </summary>
    Spammer = 1 << 6
}