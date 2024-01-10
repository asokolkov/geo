namespace Geo.Api.Client.Domain.Cities;

public sealed class CreateCityRequest
{
    public CreateCityRequest(int countryId, string name, double latitude, double longitude, string timezone, string osm)
    {
        CountryId = countryId;
        Name = name;
        Latitude = latitude;
        Longitude = longitude;
        Timezone = timezone;
        Osm = osm;
    }

    public int CountryId { get; }

    public string Name { get; }

    public double Latitude { get; }

    public double Longitude { get; }

    public string Timezone { get; }
    
    public string Osm { get; }
    
    public int? RegionId { get; init; }
    
    public string? Iata { get; init; }
}