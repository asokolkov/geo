namespace ObjectsLoader.Models;

public class Railway
{
    public Guid Id { get; init; }
    public Guid CityId { get; set; }
    public Guid CountryId { get; set; }
    public Guid RegionId { get; set; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public bool IsMain { get; init; }
    public string Rzd { get; init; } = null!;
    public string OsmId { get; init; } = null!;
    public string Timezone { get; init; } = null!;
    public string NameRu { get; init; } = null!;
    public string CityOsmId { get; init; } = null!;
}