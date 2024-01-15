namespace Geo.Api.Repositories.SubwayStations.Models;

public sealed class SubwayStationNameEntity
{
    public SubwayStationNameEntity(string en)
    {
        En = en;
    }

    public string En { get; }
    
    public string? Ru { get; init; }
}