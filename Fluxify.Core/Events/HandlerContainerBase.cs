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

using System.Collections.Immutable;

namespace Fluxify.Core.Events;

/// <summary>
/// Base class for implementing handler containers (<see cref="IHandlerContainer"/>).
/// </summary>
/// <typeparam name="THandler">The delegate type of the handlers the container holds.</typeparam>
public abstract class HandlerContainerBase<THandler> : IHandlerContainer
    where THandler : Delegate
{
    /// <summary>
    /// All handler delegates that have been inserted into the container.
    /// </summary>
    protected ImmutableArray<THandler> Handlers = [];

    void IHandlerContainer.InsertDelegate(Delegate handler)
        => InsertDelegate(handler as THandler ??
                          throw new ArgumentException($"Handler must be of type {typeof(THandler)}"));
    
    /// <inheritdoc cref="IHandlerContainer.InsertDelegate" />
    public void InsertDelegate(THandler handler) => ImmutableInterlocked.Update(
        ref Handlers,
        static (handlers, handler) => handlers.Add(handler),
        handler
    );

    void IHandlerContainer.RemoveDelegate(Delegate handler)
        => RemoveDelegate(handler as THandler ??
                          throw new ArgumentException($"Handler must be of type {typeof(THandler)}"));
    
    /// <inheritdoc cref="IHandlerContainer.RemoveDelegate" />
    public void RemoveDelegate(THandler handler) => ImmutableInterlocked.Update(
        ref Handlers,
        static (handlers, handler) => handlers.Remove(handler),
        handler
    );
}