using Geo.Api.Repositories.Cities.Models;

namespace Geo.Api.Repositories.RailwayStations.Models;

public sealed class RailwayStationEntity
{
    public RailwayStationEntity(int id, int cityId, RailwayStationCodeEntity code, RailwayStationNameEntity name,
        RailwayStationGeometryEntity geometry, string osm, bool needToUpdate, DateTimeOffset updatedAt)
    {
        Id = id;
        CityId = cityId;
        Code = code;
        Name = name;
        Geometry = geometry;
        Osm = osm;
        UpdatedAt = updatedAt;
        NeedToUpdate = needToUpdate;
    }

#pragma warning disable CS8618
    private RailwayStationEntity()
    {
    }
#pragma warning restore CS8618

    public int Id { get; }

    public int CityId { get; set; }

    public RailwayStationCodeEntity Code { get; set; }

    public RailwayStationNameEntity Name { get; set; }

    public RailwayStationGeometryEntity Geometry { get; set; }

    public int? UtcOffset { get; set; }
    
    public bool NeedToUpdate { get; set; }

    public string Osm { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public CityEntity? City { get; set; }
}