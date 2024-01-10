using Geo.Api.Domain.Cities;

namespace Geo.Api.Domain.Airports;

internal sealed class Airport
{
    public Airport(int id, Code code, Name name, Geometry geometry, AirportLocationComponents locationComponents,
        int utcOffset, string osm)
    {
        Id = id;
        Code = code;
        Name = name;
        Geometry = geometry;
        LocationComponents = locationComponents;
        UtcOffset = utcOffset;
        Osm = osm;
    }

    public int Id { get; }
    public Code Code { get; }
    public Name Name { get; }
    public Geometry Geometry { get; }
    public AirportLocationComponents LocationComponents { get; }
    public int UtcOffset { get; }
    public string Osm { get; }
}