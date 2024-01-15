using System.Text.Json.Serialization;

namespace Geo.Api.V1.Regions.Models;

public class RegionDto
{
    public RegionDto(RegionNameDto name,
        RegionLocationComponentsDto locationComponents, RegionGeometryDto geometry, string osm, bool needToUpdate,
        DateTimeOffset lastUpdate)
    {
        Name = name;
        LocationComponents = locationComponents;
        Osm = osm;
        NeedToUpdate = needToUpdate;
        LastUpdate = lastUpdate;
        Geometry = geometry;
    }

    [JsonPropertyName("name")] public RegionNameDto Name { get; }

    [JsonPropertyName("geometry")] public RegionGeometryDto Geometry { get; }

    [JsonPropertyName("utc_offset")] public int? UtcOffset { get; init; }

    [JsonPropertyName("location_components")]
    public RegionLocationComponentsDto LocationComponents { get; }

    [JsonPropertyName("osm")] public string Osm { get; }

    [JsonPropertyName("need_to_update")] public bool NeedToUpdate { get; }

    [JsonPropertyName("last_update")] public DateTimeOffset LastUpdate { get; }
}