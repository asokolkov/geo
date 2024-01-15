using System.Text.Json.Serialization;

namespace Geo.Api.V1.Airports.Models;

public sealed class AirportGeometryDto
{
    [JsonPropertyName("lat")]
    public double? Lat { get; init; }
    
    [JsonPropertyName("lon")]
    public double? Lon { get; init; }
}