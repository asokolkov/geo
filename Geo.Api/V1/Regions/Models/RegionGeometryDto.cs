using System.Text.Json.Serialization;

namespace Geo.Api.V1.Regions.Models;

public sealed class RegionGeometryDto
{
    [JsonPropertyName("lat")] public double? Lat { get; init; }

    [JsonPropertyName("lon")] public double? Lon { get; init; }
}