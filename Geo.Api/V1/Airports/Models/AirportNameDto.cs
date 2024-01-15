using System.Text.Json.Serialization;

namespace Geo.Api.V1.Airports.Models;

public sealed class AirportNameDto
{
    public AirportNameDto(string ru)
    {
        Ru = ru;
    }

    [JsonPropertyName("ru")]
    public string Ru { get; }
    
    [JsonPropertyName("en")]
    public string? En { get; init; }
}