using Geo.Api.Repositories.Cities.Models;

namespace Geo.Api.Repositories.RailwayStations.Models;

internal sealed class RailwayStationEntity
{
    public RailwayStationEntity(int id, int cityId, int rzdCode, bool isMain, string name, double latitude,
        double longitude, string timezone, string osm, DateTimeOffset updatedAt)
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
    
#pragma warning disable CS8618
    private RailwayStationEntity()
    {
    }
#pragma warning restore CS8618

    public int Id { get; }

    public int CityId { get; set; }

    public int RzdCode { get; set; }

    public bool IsMain { get; set; }

    public string Name { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public string Timezone { get; set; }

    public string Osm { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public CityEntity? City { get; set; }
}