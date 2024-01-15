using System.Text.Json.Serialization;

namespace Geo.Api.V1.Cities.Models;

public class CityDto
{
    public CityDto(CityCodeDto code, CityNameDto name,
        CityLocationComponentsDto locationComponents, CityGeometryDto geometry, string osm, bool needToUpdate,
        DateTimeOffset lastUpdate)
    {
        Code = code;
        Name = name;
        LocationComponents = locationComponents;
        Osm = osm;
        NeedToUpdate = needToUpdate;
        LastUpdate = lastUpdate;
        Geometry = geometry;
    }

    [JsonPropertyName("code")] public CityCodeDto Code { get; }

    [JsonPropertyName("name")] public CityNameDto Name { get; }

    [JsonPropertyName("geometry")] public CityGeometryDto Geometry { get; }

    [JsonPropertyName("utc_offset")] public int? UtcOffset { get; init; }

    [JsonPropertyName("location_components")]
    public CityLocationComponentsDto LocationComponents { get; }

    [JsonPropertyName("osm")] public string Osm { get; }

    [JsonPropertyName("need_to_update")] public bool NeedToUpdate { get; }

    [JsonPropertyName("last_update")] public DateTimeOffset LastUpdate { get; }
};