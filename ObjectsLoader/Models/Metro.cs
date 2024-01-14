namespace ObjectsLoader.Models;

public sealed class Metro
{
    public required string StationName { get; init; }
    public required string LineNameRu { get; init; }
    public required string LineNameEn { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public int Osm { get; init; }
}