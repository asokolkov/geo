using System.Text.Json.Serialization;

namespace Geo.Api.V1.RailStations.Models;

public class RailStationDto
{
    public RailStationDto(RailStationCodeDto code, RailStationNameDto name,
        RailStationLocationComponentsDto locationComponents, RailStationGeometryDto geometry, string osm,
        bool needToUpdate, DateTimeOffset lastUpdate)
    {
        Code = code;
        Name = name;
        LocationComponents = locationComponents;
        Osm = osm;
        NeedToUpdate = needToUpdate;
        LastUpdate = lastUpdate;
        Geometry = geometry;
    }

    [JsonPropertyName("code")] public RailStationCodeDto Code { get; }

    [JsonPropertyName("name")] public RailStationNameDto Name { get; }

    [JsonPropertyName("geometry")] public RailStationGeometryDto Geometry { get; }

    [JsonPropertyName("utc_offset")] public int? UtcOffset { get; init; }

    [JsonPropertyName("location_components")]
    public RailStationLocationComponentsDto LocationComponents { get; }

    [JsonPropertyName("osm")] public string Osm { get; }

    [JsonPropertyName("need_to_update")] public bool NeedToUpdate { get; }

    [JsonPropertyName("last_update")] public DateTimeOffset LastUpdate { get; }
};