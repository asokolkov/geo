using System.Text.Json.Serialization;

namespace Geo.Api.V1.Airports.Models;

internal sealed record AirportLocationComponentsDto(
    [property: JsonPropertyName("city_id ")]
    int CityId,
    [property: JsonPropertyName("region_id")]
    int RegionId,
    [property: JsonPropertyName("country_id")]
    int CountryId 
);