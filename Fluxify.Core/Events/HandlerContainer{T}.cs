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

namespace Fluxify.Core.Events;

public sealed class HandlerContainer<T> : IHandlerContainer
{
    private readonly HashSet<Func<T, Task>> _handlers = [];
    public void InsertDelegate(Func<T, Task> handler) => _handlers.Add(handler);
    public void RemoveDelegate(Func<T, Task> handler) => _handlers.Remove(handler);

    public async Task CallHandlersAsync(object eventPayload) => await CallHandlersAsync((T)eventPayload);
    public async Task CallHandlersAsync(T payload)
    {
        var tasks = _handlers.Select(h => h.Invoke(payload)); 
        await Task.WhenAll(tasks).ConfigureAwait(false);
    }
}