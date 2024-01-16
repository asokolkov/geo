using System.Text.Json.Serialization;
using ObjectsLoader.Extensions;

namespace ObjectsLoader.JsonModels;

public class RegionLocationComponentsJson
{
    [JsonConverter(typeof(IntToStringConverter))] [JsonPropertyName("country_id")] public string CountryId { get; init; }
}