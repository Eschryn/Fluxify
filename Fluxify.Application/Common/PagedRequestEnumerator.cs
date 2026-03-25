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

namespace Fluxify.Application.Common;

internal record Page<TIndex, TData>(TIndex? NextIndex, IReadOnlyList<TData> Content); 

internal class PagedRequestEnumerator<TData, TIndex>(
    int? limit,
    TIndex? start,
    Func<TIndex?, Task<Page<TIndex, TData>?>> requestFunc
) : IAsyncEnumerator<IReadOnlyList<TData>>
    where TIndex : struct
{
    private int _retrievedCount;
    private TIndex? _nextIndex = start;
    public IReadOnlyList<TData> Current { get; private set; } = null!;
    
    public async ValueTask<bool> MoveNextAsync()
    {
        if (_retrievedCount >= limit || await requestFunc(_nextIndex) is not {} page)
        {
            return false;
        }
        
        _retrievedCount += page.Content.Count;
        _nextIndex = page.NextIndex;
        Current = page.Content;
        return true;
    }

    public ValueTask DisposeAsync()
    {
        Current = null!;
        _nextIndex = default;
        _retrievedCount = 0;
        
        return ValueTask.CompletedTask;
    }
}