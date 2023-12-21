namespace ObjectsLoader.Models;

public sealed class Region
{
    public Guid Id { get; init; }
    public Guid CountryId { get; set; }
    public int OsmId { get; init; }
    public required string NameRu { get; init; }
    public required string IsoCode { get; init; }
}