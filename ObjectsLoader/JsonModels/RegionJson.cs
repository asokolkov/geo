using System.Text.Json.Serialization;
using ObjectsLoader.Extensions;

namespace ObjectsLoader.JsonModels;

public sealed class RegionJson
{
    [JsonConverter(typeof(IntToStringConverter))] [JsonPropertyName("id")] public string Id { get; init; }
    [JsonPropertyName("name")] public required NameJson Name { get; init; }   
    [JsonPropertyName("geometry")] public required GeometryJson Geometry { get; init; }
    [JsonPropertyName("location_components")] public required RegionLocationComponentsJson LocationComponents { get; init; }
    [JsonPropertyName("osm")] public required string Osm { get; init; }
    [JsonPropertyName("need_to_update")] public required bool NeedToUpdate { get; init; }
    [JsonPropertyName("last_update")] public required DateTimeOffset LastUpdate { get; init; }
}