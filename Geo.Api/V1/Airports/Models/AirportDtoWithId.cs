namespace Geo.Api.V1.Airports.Models;

internal sealed class AirportDtoWithId : AirportDto
{
    public AirportDtoWithId(int id, AirportCodeDto code, AirportNameDto name, AirportLocationComponentsDto locationComponents,
        AirportGeometryDto geometry, string osm, bool needToUpdate, DateTimeOffset lastUpdate) : base(code, name,
        locationComponents, geometry, osm, needToUpdate, lastUpdate)
    {
        Id = id;
    }
    
    public int Id { get; }
}