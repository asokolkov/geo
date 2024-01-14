namespace ObjectsLoader.Models;

public sealed class Country
{
    public required string Name { get; init; }
    public required string Iso3116Alpha2 { get; init; }
    public required string Iso3116Alpha3 { get; init; }
    public required string PhoneCode { get; init; }
    public required string PhoneMask { get; init; }
    public int Osm { get; init; }
}