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

using Fluxify.Dto.Webhooks;

namespace Fluxify.Application.Entities.Webhooks;

[Mapper]
internal partial class WebhookMapper(FluxerApplication fluxerApplication)
{
    private FluxerApplication App() => fluxerApplication;

    [UseMapper] private CacheMapper CacheMapper => fluxerApplication.CacheMapper;

    [MapValue("fluxerApplication", Use = nameof(App)),
     MapProperty(nameof(WebhookResponse.GuildId), nameof(Webhook.GuildRef)),
     MapProperty(nameof(WebhookResponse.ChannelId), nameof(Webhook.ChannelRef)),
     MapProperty(nameof(WebhookResponse.User), nameof(Webhook.CreatedByRef))]
    public partial Webhook FromResponse(WebhookResponse dto);

    [MapValue("fluxerApplication", Use = nameof(App)),
     MapProperty(nameof(WebhookResponse.GuildId), nameof(Webhook.GuildRef)),
     MapProperty(nameof(WebhookResponse.ChannelId), nameof(Webhook.ChannelRef)),
     MapValue(nameof(Webhook.CreatedByRef), null)]
    public partial Webhook FromResponse(WebhookTokenResponse dto);
    
    [MapperIgnoreSource(nameof(WebhookTokenResponse.Id)),
     MapperIgnoreSource(nameof(WebhookTokenResponse.GuildId)),
     MapperIgnoreSource(nameof(WebhookTokenResponse.ChannelId))]
    public partial void UpdateEntity([MappingTarget] Webhook data, WebhookTokenResponse update);
    
    [MapperRequiredMapping(RequiredMappingStrategy.Target)]
    [MapValue(nameof(WebhookProperties.Avatar), null)]
    public partial WebhookProperties ToProperties(Webhook webhook);
    
    public partial WebhookTokenUpdateRequest ToUpdateRequest(WebhookProperties properties);
}