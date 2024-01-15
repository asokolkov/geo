using System.Text.Json.Serialization;

namespace ObjectsLoader.JsonModels;

public sealed class CountryCodeJson
{
    [JsonPropertyName("iso3116_alpha2")] public required string Iso2 { get; init; }
    [JsonPropertyName("iso3116_alpha3")] public required string Iso3 { get; init; }
}