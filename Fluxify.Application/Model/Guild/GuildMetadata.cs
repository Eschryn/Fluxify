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

using Fluxify.Application.Common;
using Fluxify.Dto.Guilds.Settings;

namespace Fluxify.Application.Model.Guild;

public class GuildMetadata
{
    public Snowflake Id { get; init; }
    public string Name { get; internal set; }
    public Image? Icon { get; internal set; }
    public Image? Splash { get; internal set; }
    public Image? Banner { get; internal set; }
    public Image? EmbedSplash { get; internal set; }
    public GuildFeatureSchema[] Features { get; internal set; }
    public SplashCardAlignment SplashCardAlignment { get; internal set; }
}