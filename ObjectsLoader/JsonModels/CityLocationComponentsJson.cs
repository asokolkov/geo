using System.Text.Json.Serialization;

namespace ObjectsLoader.JsonModels;

public class CityLocationComponentsJson
{
    [JsonPropertyName("country_id")] public Guid CountryId { get; init; }
    [JsonPropertyName("region_id")] public Guid RegionId { get; init; }
}