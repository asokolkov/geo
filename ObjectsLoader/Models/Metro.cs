namespace ObjectsLoader.Models;

public sealed class Metro
{
    public required string StationNameRu { get; init; }
    public required string StationNameEn { get; init; }
    public required string LineNameRu { get; init; }
    public required string LineNameEn { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public required string Osm { get; init; }
    public required string CityOsmId { get; init; }
}