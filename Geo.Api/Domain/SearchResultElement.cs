namespace Geo.Api.Domain;

public sealed record SearchResultElement(
    string ObjectType,
    string ObjectId,
    string ObjectName,
    string CountryId,
    string CountryName)
{
    public string? RegionId { get; init; }

    public string? RegionName { get; init; }
}