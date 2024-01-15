using System.Text.Json.Serialization;

namespace Geo.Api.V1.Airports.Models;

public sealed record AirportLocationComponentsDto(
    [property: JsonPropertyName("city_id")]
    int CityId,
    [property: JsonPropertyName("country_id")]
    int CountryId
)
{
    [JsonPropertyName("region_id")]
    public int? RegionId { get; init; }
}