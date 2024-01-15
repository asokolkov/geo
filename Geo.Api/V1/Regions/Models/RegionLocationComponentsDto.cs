using System.Text.Json.Serialization;

namespace Geo.Api.V1.Regions.Models;

public sealed record RegionLocationComponentsDto(
    [property: JsonPropertyName("country_id")]
    int CountryId 
);