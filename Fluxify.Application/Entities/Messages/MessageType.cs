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

namespace Fluxify.Application.Entities.Messages;

public enum MessageType
{
    Regular = 0,
    UserAddMessage = 1,
    UserRemoveMessage = 2,
    CallMessage = 3,
    ChannelNameChangeMessage = 4,
    ChannelIconChangeMessage = 5,
    MessagePinnedMessage = 6,
    UserJoinMessage = 7,
    ReplyMessage = 19
}