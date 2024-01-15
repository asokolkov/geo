using System.Text.Json.Serialization;

namespace ObjectsLoader.JsonModels;

public sealed class MetroJson
{
    [JsonPropertyName("station_name")] public required NameJson StationName { get; init; }
    [JsonPropertyName("line_name")] public required NameJson LineName { get; init; }
    [JsonPropertyName("location_components")] public required LocationComponentsJson LocationComponents { get; init; }
    [JsonPropertyName("geometry")] public required GeometryJson Geometry { get; init; }
    [JsonPropertyName("utc_offset")] public int UtcOffset { get; init; }
    [JsonPropertyName("osm")] public required string Osm { get; init; }
    [JsonPropertyName("need_to_update")] public required bool NeedToUpdate { get; init; }
    [JsonPropertyName("last_update")] public required DateTimeOffset LastUpdate { get; init; }
}