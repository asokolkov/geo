using System.Text.Json.Serialization;

namespace ObjectsLoader.JsonModels;

public sealed class NominatimJsonElement
{
    [JsonPropertyName("ISO3166-2-lvl4")] public string? RegionIsoCode { get; init; }
    [JsonPropertyName("address")] public required Dictionary<string, string> Address { get; init; }
}