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

using Fluxify.Application.Entities.Users;
using Fluxify.Application.Model.Channel;
using Fluxify.Core.Types;

namespace Fluxify.Application.Entities.Channels.Private;

public class GroupDm(FluxerApplication fluxerApplication) : PrivateTextChannel(fluxerApplication)
{
    public string? IconHash { get; internal set; }
    public Snowflake OwnerId { get; internal set; }
    public IUser[] Recipients { get; internal set; } = [];
    public IReadOnlyDictionary<string, string> Nicks { get; internal set; } = new Dictionary<string, string>().AsReadOnly();
    public required string? Name { get; set; }
    
    public Task UpdateAsync(
        Action<GroupDmProperties> configure,
        string? reason = null,
        CancellationToken cancellationToken = default
    ) => FluxerApplication.ChannelsRepository.UpdateAsync(this, configure, reason, cancellationToken);
}