using System.Text.Json.Serialization;

namespace Geo.Api.V1.RailStations.Models;

public sealed class RailStationGeometryDto
{
    [JsonPropertyName("lat")]
    public double? Lat { get; init; }
    
    [JsonPropertyName("lon")]
    public double? Lon { get; init; }
}