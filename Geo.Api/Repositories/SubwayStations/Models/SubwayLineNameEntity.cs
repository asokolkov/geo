namespace Geo.Api.Repositories.SubwayStations.Models;

public sealed class SubwayLineNameEntity
{
    public SubwayLineNameEntity(string en)
    {
        En = en;
    }

    public string En { get; }
    
    public string? Ru { get; init; }
}