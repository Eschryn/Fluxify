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

/// <summary>
/// Generic one parameter handler container.
/// </summary>
/// <typeparam name="T">The parameter type of the handler.</typeparam>
public sealed class HandlerContainer<T> : HandlerContainerBase<Func<T, Task>>, ICallableHandlerContainer<T>
{
    /// <inheritdoc/>
    public async Task CallHandlersAsync(T payload)
        => await Task.WhenAll(
            Handlers.Select(h => h.Invoke(payload))
        ).ConfigureAwait(false);
}