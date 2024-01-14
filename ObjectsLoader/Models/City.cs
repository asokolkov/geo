namespace ObjectsLoader.Models;

public sealed class City
{
    public required string Name { get; init; }
    public required string Iata { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public required string Timezone { get; init; }
    public int Osm { get; init; }
}