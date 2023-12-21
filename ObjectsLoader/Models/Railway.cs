namespace ObjectsLoader.Models;

public sealed class Railway
{
    public Guid Id { get; init; }
    public Guid CityId { get; set; }
    public Guid CountryId { get; set; }
    public Guid RegionId { get; set; }
    public bool IsMain { get; init; }
    public int OsmId { get; init; }
    public int CityOsmId { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public required string Rzd { get; init; }
    public required string Timezone { get; init; }
    public required string NameRu { get; init; }
}