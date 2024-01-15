using System.Text.Json.Serialization;

namespace ObjectsLoader.JsonModels;

public class RegionLocationComponentsJson
{
    [JsonPropertyName("country_id")] public Guid CountryId { get; init; }
}