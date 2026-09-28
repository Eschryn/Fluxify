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

/// <summary>
/// An exception that gets thrown when the request was rate limited.
/// </summary>
/// <param name="code">Error code.</param>
/// <param name="message">The message that belongs to the error.</param>
/// <param name="retryAfter">When to retry the request.</param>
/// <param name="global">True if the global rate limit was hit. When false this error is scoped to the route rate limit.</param>
public class RateLimitException(string code, string message, int retryAfter, bool global) : Exception(message)
{
    /// <summary>
    /// The error code that represents the reason.
    /// </summary>
    public string Code { get; } = code;

    /// <summary>
    /// When to retry the request.
    /// </summary>
    public int RetryAfter { get; } = retryAfter;

    /// <summary>
    /// True if the global rate limit was hit. When false this error is scoped to the route rate limit.
    /// </summary>
    public bool Global { get; } = global;
}