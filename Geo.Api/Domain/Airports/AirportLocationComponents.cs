namespace Geo.Api.Domain.Airports;

internal sealed record AirportLocationComponents(int CityId, int CountryId)
{
    public int? RegionId { get; init; }
};