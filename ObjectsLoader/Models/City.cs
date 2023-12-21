namespace ObjectsLoader.Models;

public sealed class City
{
    public Guid Id { get; init; }
    public Guid CountryId { get; set; }
    public Guid RegionId { get; set; }
    public int OsmId { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public required string NameRu { get; init; }
    public required string Timezone { get; init; }
    public required string RegionIsoCode { get; init; }
}