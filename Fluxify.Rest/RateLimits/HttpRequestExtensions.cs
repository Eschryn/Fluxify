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

namespace Fluxify.Rest.RateLimits;

internal static class HttpRequestExtensions
{
    private static readonly HttpRequestOptionsKey<string> Bucket = new("792F5737-E5E4-4D97-AF76-520CD4C4A7A9.rate-limit");

    extension(HttpRequestMessage request)
    {
        public void SetBucketIfNotNull(string? bucket)
        {
            if (bucket != null)
            {
                request.Options.Set(Bucket, bucket);
            }
        }

        public bool TryGetBucket(out string? bucket)
        {
            return request.Options.TryGetValue(Bucket, out bucket);
        }
    }
}