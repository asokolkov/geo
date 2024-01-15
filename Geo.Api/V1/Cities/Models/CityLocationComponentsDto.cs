using System.Text.Json.Serialization;

namespace Geo.Api.V1.Cities.Models;

public sealed record CityLocationComponentsDto(
    [property: JsonPropertyName("country_id")]
    int CountryId
)
{
    [JsonPropertyName("region_id")]
    public int? RegionId { get; init; }   
}