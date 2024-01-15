namespace Geo.Api.V1.RailStations.Models;

internal sealed class RailStationDtoWithId : RailStationDto
{
    public RailStationDtoWithId(int id, RailStationCodeDto code, RailStationNameDto name,
        RailStationLocationComponentsDto locationComponents, RailStationGeometryDto geometry, string osm,
        bool needToUpdate, DateTimeOffset lastUpdate) :
        base(code, name, locationComponents, geometry, osm, needToUpdate, lastUpdate)
    {
        Id = id;
    }

    public int Id { get; }
}