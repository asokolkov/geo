using System.Text.Json.Serialization;

namespace Geo.Api.V1.RailStations.Models;

public sealed class RailStationNameDto
{
    public RailStationNameDto(string en)
    {
        En = en;
    }

    [JsonPropertyName("ru")] public string? Ru { get; init; }

    [JsonPropertyName("en")] public string En { get; }
}