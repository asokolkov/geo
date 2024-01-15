namespace ObjectsLoader.Models;

public sealed class Airport
{
    public required string NameEn { get; init; }
    public required string NameRu { get; init; }
    public string? IataRu { get; init; }
    public required string IataEn { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public int? UtcOffset { get; init; }
    public required string Osm { get; init; }
    public required string CityOsmId { get; init; }
}