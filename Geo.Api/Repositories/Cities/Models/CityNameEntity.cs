namespace Geo.Api.Repositories.Cities.Models;

public sealed class CityNameEntity
{
    public CityNameEntity(string ru)
    {
        Ru = ru;
    }
    
    public string Ru { get; }
    
    public string? En { get; init; }
}