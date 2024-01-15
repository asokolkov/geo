namespace ObjectsLoader.Models;

public sealed class City
{
    public required string NameRu { get; init; }
    public required string NameEn { get; init; }
    public required string RegionIsoCode { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public int? UtcOffset { get; init; }
    public required string Osm { get; init; }
}