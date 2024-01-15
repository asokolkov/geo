using System.Text.Json.Serialization;

namespace Geo.Api.V1.SubwayStations.Models;

public sealed class SubwayLineNameDto
{
    public SubwayLineNameDto(string en)
    {
        En = en;
    }

    [JsonPropertyName("ru")]
    public string? Ru { get; init; }
    
    [JsonPropertyName("en")]
    public string En { get; }
}