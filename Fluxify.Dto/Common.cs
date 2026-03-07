using System.Text.Json.Serialization;
using Fluxify.Core;

namespace Fluxify.Dto;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ConnectionType
{
    Bsky,
    Domain 
}

public record ConnectionResponse(
    Snowflake Id,
    string Name,
    int SortOrder,
    ConnectionType Type,
    bool Verified,
    int VisibilityFlags // TODO: What is this?
    );

[JsonConverter(typeof(JsonStringEnumConverter))]
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

public enum RenderSpoilers
{
    Always = 0,
    OnClick = 1,
    WhenModerator = 2
}

public enum TimeFormatTypes
{
    Automatic = 0,
    H12 = 1,
    H24 = 2
}

public enum InviteType
{
    Guild = 0,
    GroupDm = 1
}