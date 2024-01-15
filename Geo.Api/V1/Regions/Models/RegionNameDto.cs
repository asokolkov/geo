using System.Text.Json.Serialization;

namespace Geo.Api.V1.Regions.Models;

public sealed class RegionNameDto
{
    public RegionNameDto(string ru)
    {
        Ru = ru;
    }

    [JsonPropertyName("ru")]
    public string Ru { get; }
    
    [JsonPropertyName("en")]
    public string? En { get; init; }
}