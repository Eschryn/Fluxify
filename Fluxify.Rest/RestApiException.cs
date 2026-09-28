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

using Fluxify.Rest.Model;

namespace Fluxify.Rest;

/// <summary>
/// An exception that gets thrown when something went wrong or the server rejected the request.
/// </summary>
/// <param name="code">Error code that represents the reason why the request was rejected.</param>
/// <param name="message">Error message that contains the reason why the request was rejected in human readable form.</param>
/// <param name="errors">Property/field specific errors.</param>
public class RestApiException(string code, string message, Error[] errors) : Exception(message)
{
    /// <summary>
    /// Error code that represents the reason why the request was rejected.
    /// </summary>
    public string Code { get; } = code;
    
    /// <summary>
    /// Property/field specific errors.
    /// </summary>
    public Error[] Errors { get; } = errors;
}