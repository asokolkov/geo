namespace ObjectsLoader.Models;

public sealed class Region
{
    public Guid Id { get; init; }
    public Guid CountryId { get; set; }
    public int OsmId { get; init; }
    public string NameRu { get; init; } = null!;
    public string IsoCode { get; init; } = null!;
}