namespace ObjectsLoader.Models;

public sealed class Railway
{
    public required string Express3Code { get; init; }
    public bool IsMain { get; init; }
    public required string Name { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public required string Timezone { get; init; }
    public int Osm { get; init; }
}