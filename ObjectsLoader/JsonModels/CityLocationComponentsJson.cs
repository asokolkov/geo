using System.Text.Json.Serialization;
using ObjectsLoader.Extensions;

namespace ObjectsLoader.JsonModels;

public class CityLocationComponentsJson
{
    [JsonConverter(typeof(IntToStringConverter))] [JsonPropertyName("country_id")] public string CountryId { get; init; }
    [JsonConverter(typeof(IntToStringConverter))] [JsonPropertyName("region_id")] public string RegionId { get; init; }
}