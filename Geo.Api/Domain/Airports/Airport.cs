using Geo.Api.Domain.Cities;

namespace Geo.Api.Domain.Airports;

internal sealed class Airport
{
    public Airport(int id, int cityId, string name, string iataEn, double latitude, double longitude,
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

    public int Id { get; }
    
    public int CityId { get; }

    public string Name { get; }

    public string IataEn { get; }

    public double Latitude { get; }

    public double Longitude { get; }

    public string Timezone { get; }
    
    public string Osm { get; }

    public DateTimeOffset UpdatedAt { get; }

    public string? IataRu { get; init; }

    public DateTimeOffset? DeletedAt { get; init; }

    public City? City { get; init; }
}