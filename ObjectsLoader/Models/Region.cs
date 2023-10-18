namespace ObjectsLoader.Models;

public class Region
{
    public Guid Id { get; init; }
    public Guid CountryId { get; set; }
    public string OsmId { get; init; } = null!;
    public string NameRu { get; init; } = null!;
    public string IsoCode { get; init; } = null!;
}