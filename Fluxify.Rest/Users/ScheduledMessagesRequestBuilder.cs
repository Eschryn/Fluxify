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

using System.Globalization;
using System.Text;
using Fluxify.Core.Types;
using Fluxify.Dto.Users.ScheduledMessages;

namespace Fluxify.Rest.Users;

public class ScheduledMessagesRequestBuilder(HttpClient client)
{
    private static readonly IFormatProvider FormatProvider = CultureInfo.InvariantCulture;
    private const string ScheduledMessagesUrl = "users/@me/scheduled-messages";
    private static readonly CompositeFormat ScheduledMessageUrl = CompositeFormat.Parse("users/@me/scheduled-messages/{0}");
    
    public Task<ScheduleMessageResponseSchema[]> ListScheduledMessagesAsync(
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<ScheduleMessageResponseSchema[]>(
        HttpMethod.Get,
        ScheduledMessagesUrl,
        cancellationToken: cancellationToken
    );
    
    public Task<ScheduleMessageResponseSchema> GetScheduledMessageAsync(
        Snowflake messageId,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<ScheduleMessageResponseSchema>(
        HttpMethod.Get,
        string.Format(FormatProvider, ScheduledMessageUrl, messageId),
        cancellationToken: cancellationToken
    );
    
    public Task DeleteScheduledMessageAsync(
        Snowflake messageId,
        CancellationToken cancellationToken = default
    ) => client.RequestAsync(
        HttpMethod.Delete,
        string.Format(FormatProvider, ScheduledMessageUrl, messageId),
        cancellationToken: cancellationToken
    );
    
    public Task<ScheduleMessageResponseSchema> UpdateScheduledMessageAsync(
        Snowflake scheduledMessageId,
        ScheduledMessageSchema request,
        CancellationToken cancellationToken = default
    ) => client.MultipartJsonRequestAsync<ScheduledMessageSchema, ScheduleMessageResponseSchema>(
        HttpMethod.Patch,
        string.Format(FormatProvider, ScheduledMessageUrl, scheduledMessageId),
        request,
        cancellationToken: cancellationToken
    );
}