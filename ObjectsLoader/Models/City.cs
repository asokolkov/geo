namespace ObjectsLoader.Models;

public class City
{
    public Guid Id { get; init; }
    public Guid CountryId { get; set; }
    public Guid RegionId { get; set; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public string OsmId { get; init; } = null!;
    public string NameRu { get; init; } = null!;
    public string Timezone { get; init; } = null!;
    public string? IataEn { get; init; }
    public string CountryIsoCode { get; init; } = null!;
    public string RegionIsoCode { get; init; } = null!;
}