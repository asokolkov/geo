namespace Geo.Api.Repositories.Regions.Models;

public sealed class RegionNameEntity
{
    public RegionNameEntity(string ru)
    {
        Ru = ru;
    }
    
    public string Ru { get; }
    
    public string? En { get; init; }
}