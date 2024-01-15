using System.Text.Json.Serialization;

namespace Geo.Api.V1.SubwayStations.Models;

public class SubwayStationDto
{
    public SubwayStationDto(SubwayStationNameDto stationName, SubwayLineNameDto lineName,
        SubwayLocationComponentsDto locationComponents, SubwayGeometryDto geometry, string osm, bool needToUpdate, DateTimeOffset lastUpdate)
    {
        StationName = stationName;
        LineName = lineName;
        LocationComponents = locationComponents;
        Osm = osm;
        NeedToUpdate = needToUpdate;
        LastUpdate = lastUpdate;
        Geometry = geometry;
    }

    [JsonPropertyName("station_name")] public SubwayStationNameDto StationName { get; }

    [JsonPropertyName("line_name")] public SubwayLineNameDto LineName { get; }

    [JsonPropertyName("geometry")] public SubwayGeometryDto Geometry { get; }
    
    [JsonPropertyName("location_components")]
    public SubwayLocationComponentsDto LocationComponents { get; }

    [JsonPropertyName("osm")] public string Osm { get; }

    [JsonPropertyName("need_to_update")] public bool NeedToUpdate { get; }

    [JsonPropertyName("last_update")] public DateTimeOffset LastUpdate { get; }
};