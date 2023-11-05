using Geo.Api.Repositories.Cities.Models;

namespace Geo.Api.Repositories.Airports.Models;

internal sealed class AirportEntity
{
    public AirportEntity(int id, int cityId, string name, string iataEn, double latitude, double longitude,
        string timezone, string osm, DateTimeOffset updatedAt)
    {
        Id = id;
        CityId = cityId;
        Name = name;
        IataEn = iataEn;
        Latitude = latitude;
        Longitude = longitude;
        Timezone = timezone;
        Osm = osm;
        UpdatedAt = updatedAt;
    }

#pragma warning disable CS8618
    private AirportEntity()
    {
    }
#pragma warning restore CS8618

    public int Id { get; }

    public int CityId { get; set; }

    public string Name { get; set; }

    public string IataEn { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public string Timezone { get; set; }

    public string Osm { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public string? IataRu { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public CityEntity? City { get; init; }
}