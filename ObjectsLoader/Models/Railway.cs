namespace ObjectsLoader.Models;

public sealed class Railway
{
    public required string Express3Code { get; init; }
    public bool IsMain { get; init; }
    public required string NameRu { get; init; }
    public required string NameEn { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public int? UtcOffset { get; init; }
    public required string Osm { get; init; }
    public required string CityOsmId { get; init; }
}