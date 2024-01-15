using System.Text.Json.Serialization;

namespace ObjectsLoader.JsonModels;

public class LocationComponentsJson
{
    [JsonPropertyName("country_id")] public Guid CountryId { get; init; }
    [JsonPropertyName("region_id")] public Guid RegionId { get; init; }
    [JsonPropertyName("city_id")] public Guid CityId { get; init; }
}