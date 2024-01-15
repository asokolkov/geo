using System.Text.Json.Serialization;

namespace Geo.Api.V1.Cities.Models;

public sealed class CityGeometryDto
{
    [JsonPropertyName("lat")]
    public double? Lat { get; init; }
    
    [JsonPropertyName("lon")]
    public double? Lon { get; init; }
}