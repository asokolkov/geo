using System.Text.Json.Serialization;

namespace Geo.Api.V1.Airports.Models;

public class AirportDto
{
    public AirportDto(AirportCodeDto code, AirportNameDto name,
        AirportLocationComponentsDto locationComponents, AirportGeometryDto geometry, string osm, bool needToUpdate,
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

    [JsonPropertyName("code")] public AirportCodeDto Code { get; }

    [JsonPropertyName("name")] public AirportNameDto Name { get; }

    [JsonPropertyName("geometry")] public AirportGeometryDto Geometry { get; }

    [JsonPropertyName("utc_offset")] public int? UtcOffset { get; init; }

    [JsonPropertyName("location_components")]
    public AirportLocationComponentsDto LocationComponents { get; }

    [JsonPropertyName("osm")] public string Osm { get; }

    [JsonPropertyName("need_to_update")] public bool NeedToUpdate { get; }

    [JsonPropertyName("last_update")] public DateTimeOffset LastUpdate { get; }
};