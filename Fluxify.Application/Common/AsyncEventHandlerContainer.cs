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

using Fluxify.Core.Events;

namespace Fluxify.Application.Common;

/// <summary>
/// Handler container for <see cref="AsyncEventHandler{TEventArgs}"/>.
/// </summary>
/// <typeparam name="TEventArgs">The type of the event argument container.</typeparam>
public class AsyncEventHandlerContainer<TEventArgs> 
    : HandlerContainerBase<AsyncEventHandler<TEventArgs>>,
        ICallableHandlerContainer<FluxerApplication, TEventArgs>
{
    /// <inheritdoc />
    public async Task CallHandlersAsync(FluxerApplication application, TEventArgs eventPayload) 
        => await Task.WhenAll(BeginInvokeHandlers(application, eventPayload)).ConfigureAwait(false);

    private IEnumerable<Task> BeginInvokeHandlers(FluxerApplication application, TEventArgs eventPayload)
    {
        var asyncEventHandlers = Handlers;

        foreach (var asyncEventHandler in asyncEventHandlers)
        {
            yield return asyncEventHandler.Invoke(application, eventPayload);
        }
    }
}
