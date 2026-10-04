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
/// Represents information about a scheduled deletion of every message the user sent.
/// </summary>
/// <param name="ChannelCount">The number of channels holding messages that will be deleted.</param>
/// <param name="MessageCount">The number of messages that will be deleted.</param>
/// <param name="ScheduledAt">When the deletion occurs.</param>
/// <seealso href="https://docs.fluxer.app/http-api/users/#pending-bulk-message-deletion-object"/>
public record PendingBulkMessageDeletion(
    int ChannelCount,
    int MessageCount,
    DateTimeOffset ScheduledAt
);