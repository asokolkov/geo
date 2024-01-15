using System.Text.Json.Serialization;

namespace Geo.Api.V1.SubwayStations.Models;

public sealed class SubwayGeometryDto
{
    [JsonPropertyName("lat")]
    public double? Lat { get; init; }
    
    [JsonPropertyName("lon")]
    public double? Lon { get; init; }
}