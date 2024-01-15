using Geo.Api.Repositories.Cities.Models;

namespace Geo.Api.Repositories.SubwayStations.Models;

public sealed class SubwayStationEntity
{
    public SubwayStationEntity(int id, SubwayStationNameEntity stationName, SubwayLineNameEntity lineName,
        SubwayStationGeometryEntity geometry, int cityId, string osm, bool needToUpdate, DateTimeOffset updatedAt)
    {
        Id = id;
        StationName = stationName;
        LineName = lineName;
        Geometry = geometry;
        CityId = cityId;
        UpdatedAt = updatedAt;
        Osm = osm;
        NeedToUpdate = needToUpdate;
    }

    public int Id { get; }

    public SubwayStationNameEntity StationName { get; set; }

    public SubwayLineNameEntity LineName { get; set; }

    public SubwayStationGeometryEntity Geometry { get; set; }

    public int CityId { get; set; }
    
    public string Osm { get; set; }

    public CityEntity? City { get; init; }
    
    public bool NeedToUpdate { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }
    
    public DateTimeOffset? DeletedAt { get; set; }
}