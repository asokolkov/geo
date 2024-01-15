namespace Geo.Api.V1.SubwayStations.Models;

internal sealed class SubwayStationDtoWithId : SubwayStationDto
{
    public SubwayStationDtoWithId(int id, SubwayStationNameDto stationName, SubwayLineNameDto lineName,
        SubwayLocationComponentsDto locationComponents, SubwayGeometryDto geometry, string osm, bool needToUpdate, DateTimeOffset lastUpdate) :
        base(stationName, lineName, locationComponents, geometry, osm, needToUpdate, lastUpdate)
    {
        Id = id;
    }
    
    public int Id { get; }
}