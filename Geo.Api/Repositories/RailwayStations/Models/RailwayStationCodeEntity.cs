namespace Geo.Api.Repositories.RailwayStations.Models;

public sealed class RailwayStationCodeEntity
{
    public RailwayStationCodeEntity(int express3)
    {
        Express3 = express3;
    }
    
    public int Express3 { get; }
}