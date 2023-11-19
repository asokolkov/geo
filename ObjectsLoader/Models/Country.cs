namespace ObjectsLoader.Models;

public sealed class Country
{
    public Guid Id { get; init; }
    public string OsmId { get; init; } = null!;
    public string NameRu { get; init; } = null!;
    public string PhoneCode { get; init; } = null!;
    public string PhoneMask { get; init; } = null!;
    public string Iso2 { get; init; } = null!;
    public string Iso3 { get; init; } = null!;
}