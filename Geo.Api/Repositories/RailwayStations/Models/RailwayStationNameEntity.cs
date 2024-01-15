namespace Geo.Api.Repositories.RailwayStations.Models;

public sealed class RailwayStationNameEntity
{
    public RailwayStationNameEntity(string en)
    {
        En = en;
    }
    
    public string En { get; }
    
    public string? Ru { get; init; }
}