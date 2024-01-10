namespace Geo.Api.Client.Domain.Cities;

public sealed class UpdateCityRequest
{
    public UpdateCityRequest(CityId id, int countryId, string name, double latitude, double longitude, string timezone, string osm)
    {
        Id = id;
        CountryId = countryId;
        Name = name;
        Latitude = latitude;
        Longitude = longitude;
        Timezone = timezone;
        Osm = osm;
    }
    
    public CityId Id { get; }

    public int CountryId { get; }

    public string Name { get; }

    public double Latitude { get; }

    public double Longitude { get; }

    public string Timezone { get; }
    
    public string Osm { get; }
    
    public int? RegionId { get; init; }
    
    public string? Iata { get; init; }
}