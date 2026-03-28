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

using System.Text;

namespace Fluxify.Rest;

public class QueryBuilder
{
    public StringBuilder QueryStringBuilder { get; } = new("?");
    
    public QueryBuilder AddQuery(string key, string? value)
    {
        if (value != null)
        {
            QueryStringBuilder.Append($"{key}={value}&");
        }
        
        return this;
    }
    
    public static implicit operator string(QueryBuilder builder) 
        => builder.QueryStringBuilder
            .Remove(builder.QueryStringBuilder.Length - 1, 1)
            .ToString();
    
    public override string ToString() => this;

    public QueryBuilder AddOptional(string key, bool add)
    {
        if (add)
        {
            QueryStringBuilder.Append($"{key}&");
        }
        return this;
    }
}