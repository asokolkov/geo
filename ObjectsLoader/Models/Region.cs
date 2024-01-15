namespace ObjectsLoader.Models;

public sealed class Region
{
    public required string NameRu { get; init; }
    public required string NameEn { get; init; }
    public required string Osm { get; init; }
    public required string CountryIso2Code { get; init; }
    public required string Iso { get; init; }
}