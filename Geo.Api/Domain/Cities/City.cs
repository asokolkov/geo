using Geo.Api.Domain.Countries;
using Geo.Api.Domain.Regions;

namespace Geo.Api.Domain.Cities;

internal sealed class City
{
    public City(int id, int countryId, string name, double latitude, double longitude,
        string timezone, string osm, DateTimeOffset updatedAt)
    {
        Id = id;
        CountryId = countryId;
        Name = name;
        Latitude = latitude;
        Longitude = longitude;
        Timezone = timezone;
        Osm = osm;
        UpdatedAt = updatedAt;
    }

    public int Id { get; }
    
    public int CountryId { get; }

    public string Name { get; }

    public double Latitude { get; }

    public double Longitude { get; }

    public string Timezone { get; }
    
    public string Osm { get; }
    
    public int? RegionId { get; init; }

    public DateTimeOffset UpdatedAt { get; }

    public string? Iata { get; init; }

    public DateTimeOffset? DeletedAt { get; init; }

    public Region? Region { get;init; }

    public Country? Country { get; init; }
}