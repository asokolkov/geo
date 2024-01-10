namespace Geo.Api.Client.Domain.Airports;

public sealed class UpdateAirportRequest
{
    public UpdateAirportRequest(AirportId id, int cityId, string name, string iataEn, double latitude, double longitude, string timezone, string osm)
    {
        Id = id;
        CityId = cityId;
        Name = name;
        IataEn = iataEn;
        Latitude = latitude;
        Longitude = longitude;
        Timezone = timezone;
        Osm = osm;
    }
    
    public AirportId Id { get; }
    
    public int CityId { get; }
    
    public string Name { get; }
    
    public string IataEn { get; }
    
    public string? IataRu { get; init; }
    
    public double Latitude { get; }
    
    public double Longitude { get; }
    
    public string Timezone { get; }
    
    public string Osm { get; }
}