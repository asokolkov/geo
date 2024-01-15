namespace ObjectsLoader.Models;

public sealed class Country
{
    public Guid Id { get; init; }
    public required string NameRu { get; init; }
    public required string NameEn { get; init; }
    public required string Iso3116Alpha2 { get; init; }
    public required string Iso3116Alpha3 { get; init; }
    public required string PhoneCode { get; init; }
    public required string PhoneMask { get; init; }
    public required string Osm { get; init; }
}