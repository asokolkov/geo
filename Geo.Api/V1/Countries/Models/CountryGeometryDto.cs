using System.Text.Json.Serialization;

namespace Geo.Api.V1.Countries.Models;

public sealed class CountryGeometryDto
{
    [JsonPropertyName("lat")]
    public double? Lat { get; init; }
    
    [JsonPropertyName("lon")]
    public double? Lon { get; init; }
}