namespace ObjectsLoader.Models;

public sealed class Country
{
    public Guid Id { get; init; }
    public int OsmId { get; init; }
    public required string NameRu { get; init; }
    public required string PhoneCode { get; init; }
    public required string PhoneMask { get; init; }
    public required string Iso2 { get; init; }
    public required string Iso3 { get; init; }
}