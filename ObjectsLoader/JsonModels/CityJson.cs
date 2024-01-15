using System.Text.Json.Serialization;

namespace ObjectsLoader.JsonModels;

public sealed class CityJson
{
    [JsonPropertyName("id")] public Guid Id { get; init; }   
    [JsonPropertyName("code")] public required CodeJson Code { get; init; }   
    [JsonPropertyName("name")] public required NameJson Name { get; init; }   
    [JsonPropertyName("geometry")] public required GeometryJson Geometry { get; init; }
    [JsonPropertyName("utc_offset")] public int UtcOffset { get; init; }
    [JsonPropertyName("location_components")] public required CityLocationComponentsJson LocationComponentsJson { get; init; }
    [JsonPropertyName("osm")] public required string Osm { get; init; }
    [JsonPropertyName("need_to_update")] public required bool NeedToUpdate { get; init; }
    [JsonPropertyName("last_update")] public required DateTimeOffset LastUpdate { get; init; }
}