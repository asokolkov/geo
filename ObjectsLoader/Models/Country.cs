namespace ObjectsLoader.Models;

public sealed class Country
{
    public Guid Id { get; init; }
    public int OsmId { get; init; }
    public string NameRu { get; init; } = null!;
    public string PhoneCode { get; init; } = null!;
    public string PhoneMask { get; init; } = null!;
    public string Iso2 { get; init; } = null!;
    public string Iso3 { get; init; } = null!;
}