using Geo.Api.Repositories.Cities.Models;

namespace Geo.Api.Repositories.Airports.Models;

public sealed class AirportEntity
{
    public AirportEntity(int id, int cityId, AirportNameEntity name, AirportCodeEntity code, AirportGeometryEntity geometry,
        string osm, bool needAutomaticUpdate, DateTimeOffset updatedAt)
    {
        Id = id;
        CityId = cityId;
        Name = name;
        Code = code;
        Geometry = geometry;
        Osm = osm;
        UpdatedAt = updatedAt;
        NeedAutomaticUpdate = needAutomaticUpdate;
    }

#pragma warning disable CS8618
    private AirportEntity()
    {
    }
#pragma warning restore CS8618

    public int Id { get; }

    public int CityId { get; set; }

    public AirportNameEntity Name { get; set; }

    public AirportCodeEntity Code { get; set; }

    public AirportGeometryEntity Geometry { get; set; }

    public int? UtcOffset { get; set; }

    public string Osm { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }
    
    public bool NeedAutomaticUpdate { get; set; }

    public CityEntity? City { get; init; }
}