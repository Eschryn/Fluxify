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

using System.Text.Json.Serialization;

namespace Fluxify.Dto.Reports;

[JsonConverter(typeof(JsonStringEnumConverter<Country>))]
public enum Country
{
    [JsonStringEnumMemberName("AT")]
    Austria,
    [JsonStringEnumMemberName("BE")]
    Belgium,
    [JsonStringEnumMemberName("BG")]
    Bulgaria,
    [JsonStringEnumMemberName("HR")]
    Croatia,
    [JsonStringEnumMemberName("CY")]
    Cyprus,
    [JsonStringEnumMemberName("CZ")]
    Czechia,
    [JsonStringEnumMemberName("DK")]
    Denmark,
    [JsonStringEnumMemberName("EE")]
    Estonia,
    [JsonStringEnumMemberName("FI")]
    Finland,
    [JsonStringEnumMemberName("FR")]
    France,
    [JsonStringEnumMemberName("DE")]
    Germany,
    [JsonStringEnumMemberName("GR")]
    Greece,
    [JsonStringEnumMemberName("HU")]
    Hungary,
    [JsonStringEnumMemberName("IE")]
    Ireland,
    [JsonStringEnumMemberName("IT")]
    Italy,
    [JsonStringEnumMemberName("LV")]
    Latvia,
    [JsonStringEnumMemberName("LT")]
    Lithuania,
    [JsonStringEnumMemberName("LU")]
    Luxembourg,
    [JsonStringEnumMemberName("MT")]
    Malta,
    [JsonStringEnumMemberName("NL")]
    Netherlands,
    [JsonStringEnumMemberName("PL")]
    Poland,
    [JsonStringEnumMemberName("PT")]
    Portugal,
    [JsonStringEnumMemberName("RO")]
    Romania,
    [JsonStringEnumMemberName("SK")]
    Slovakia,
    [JsonStringEnumMemberName("SI")]
    Slovenia,
    [JsonStringEnumMemberName("ES")]
    Spain,
    [JsonStringEnumMemberName("SE")]
    Sweden,
}