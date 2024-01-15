namespace Geo.Api.Repositories.Countries.Models;

public sealed class CountryNameEntity
{
    public CountryNameEntity(string ru)
    {
        Ru = ru;
    }
    
    public string Ru { get; }
    
    public string? En { get; init; }
}