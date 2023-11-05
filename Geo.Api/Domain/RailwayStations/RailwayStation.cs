using Geo.Api.Domain.Cities;

namespace Geo.Api.Domain.RailwayStation;

internal sealed class RailwayStation
{
    public RailwayStation(int id, int cityId, int rzdCode, bool isMain, string name, double latitude, double longitude, string timezone, string osm, DateTimeOffset updatedAt)
    {
        Id = id;
        CityId = cityId;
        RzdCode = rzdCode;
        IsMain = isMain;
        Name = name;
        Latitude = latitude;
        Longitude = longitude;
        Timezone = timezone;
        Osm = osm;
        UpdatedAt = updatedAt;
    }

    public int Id { get; }
    
    public int CityId { get; }

    public int RzdCode { get; }
    
    public bool IsMain { get; }
    
    public string Name { get; }
    
    public double Latitude { get; }
    
    public double Longitude { get; }
    
    public string Timezone { get; }
    
    public string Osm { get; }

    public DateTimeOffset UpdatedAt { get; }
    
    public DateTimeOffset? DeletedAt { get; init; }
    
    public City? City { get; init; }
}