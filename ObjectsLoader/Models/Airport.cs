namespace ObjectsLoader.Models;

public sealed class Airport
{
    public Guid Id { get; init; }
    public Guid CityId { get; set; }
    public Guid CountryId { get; set; }
    public Guid RegionId { get; set; }
    public int OsmId { get; init; }    
    public int CityOsmId { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public string? IataRu { get; init; }
    public required string Timezone { get; init; }
    public required string NameRu { get; init; }
    public required string IataEn { get; init; }
}