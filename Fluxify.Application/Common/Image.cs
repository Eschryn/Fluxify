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
using Fluxify.Application.Model;

namespace Fluxify.Application.Common;

public readonly record struct Image(
    Uri BaseUri,
    int? Width,
    int? Height
)
{
    private Uri BaseUri { get; init; } = BaseUri;

    private static CompositeFormat QueryParamsFormat { get; } =
        CompositeFormat.Parse("{0}?size={1}&quality={2}&format={0}&animated={3}");

    public Uri GetUri(
        int size = 128,
        ImageFormat format = ImageFormat.Webp,
        ImageQuality quality = ImageQuality.High,
        bool animated = false
    ) => new(
        BaseUri,
        string.Format(
            CultureInfo.InvariantCulture,
            QueryParamsFormat,
            format.ToString().ToLowerInvariant(),
            size,
            quality.ToString().ToLowerInvariant(),
            animated.ToString().ToLowerInvariant()
        )
    );
};