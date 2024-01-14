namespace ObjectsLoader.Models;

public sealed class Airport
{
    public required string Name { get; init; }
    public string? IataRu { get; init; }
    public required string IataEn { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public required string Timezone { get; init; }
    public int Osm { get; init; }
}