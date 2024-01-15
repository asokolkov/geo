using System.Text.Json.Serialization;

namespace Geo.Api.V1.SubwayStations.Models;

public sealed record SubwayLocationComponentsDto(
    [property: JsonPropertyName("city_id")]
    int CityId,
    [property: JsonPropertyName("country_id")]
    int CountryId
)
{
    [JsonPropertyName("region_id")]
    public int? RegionId { get; init; }
}