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
using Fluxify.Dto.Json;

namespace Fluxify.Dto.Common;

[JsonConverter(typeof(JsonLowerCaseStringEnumConverter<Locale>))]
public enum Locale
{
    [JsonStringEnumMemberName("ar")]
    Arabic,
    [JsonStringEnumMemberName("bg")]
    Bulgarian,
    [JsonStringEnumMemberName("cs")]
    Czech,
    [JsonStringEnumMemberName("da")]
    Danish,
    [JsonStringEnumMemberName("de")]
    German,
    [JsonStringEnumMemberName("el")]
    Greek,
    [JsonStringEnumMemberName("en-GB")]
    EnglishUk,
    [JsonStringEnumMemberName("en-US")]
    EnglishUs,
    [JsonStringEnumMemberName("es-ES")]
    SpanishSpain,
    [JsonStringEnumMemberName("es-419")]
    SpanishLatAm,
    [JsonStringEnumMemberName("fi")]
    Finnish,
    [JsonStringEnumMemberName("fr")]
    French,
    [JsonStringEnumMemberName("he")]
    Hebrew,
    [JsonStringEnumMemberName("hi")]
    Hindi,
    [JsonStringEnumMemberName("hr")]
    Croatian,
    [JsonStringEnumMemberName("hu")]
    Hungarian,
    [JsonStringEnumMemberName("id")]
    Indonesian,
    [JsonStringEnumMemberName("it")]
    Italian,
    [JsonStringEnumMemberName("ja")]
    Japanese,
    [JsonStringEnumMemberName("ko")]
    Korean,
    [JsonStringEnumMemberName("lt")]
    Lithuanian,
    [JsonStringEnumMemberName("nl")]
    Dutch,
    [JsonStringEnumMemberName("no")]
    Norwegian,
    [JsonStringEnumMemberName("pl")]
    Polish,
    [JsonStringEnumMemberName("pt-BR")]
    PortugueseBrazil,
    [JsonStringEnumMemberName("pt-PT")]
    PortuguesePortugal,
    [JsonStringEnumMemberName("ro")]
    Romanian,
    [JsonStringEnumMemberName("ru")]
    Russian,
    [JsonStringEnumMemberName("sv-SE")]
    Swedish,
    [JsonStringEnumMemberName("th")]
    Thai,
    [JsonStringEnumMemberName("tr")]
    Turkish,
    [JsonStringEnumMemberName("uk")]
    Ukrainian,
    [JsonStringEnumMemberName("vi")]
    Vietnamese,
    [JsonStringEnumMemberName("zh-CN")]
    ChineseChina,
    [JsonStringEnumMemberName("zh-TW")]
    ChineseTaiwan,
}