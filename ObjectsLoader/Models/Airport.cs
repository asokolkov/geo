namespace ObjectsLoader.Models;

public sealed class Airport
{
    public Guid Id { get; init; }
    public Guid CityId { get; set; }
    public Guid CountryId { get; set; }
    public Guid RegionId { get; set; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public string OsmId { get; init; } = null!;
    public string Timezone { get; init; } = null!;
    public string NameRu { get; init; } = null!;
    public string IataEn { get; init; } = null!;
    public string? IataRu { get; init; }
    public string CityOsmId { get; init; } = null!;
}