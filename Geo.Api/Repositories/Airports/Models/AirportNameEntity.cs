namespace Geo.Api.Repositories.Airports.Models;

public sealed class AirportNameEntity
{
    public AirportNameEntity(string ru)
    {
        Ru = ru;
    }
    
    public string? En { get; init; }
    
    public string Ru { get; }
}