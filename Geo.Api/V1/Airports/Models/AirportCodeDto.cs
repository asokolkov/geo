using System.Text.Json.Serialization;

namespace Geo.Api.V1.Airports.Models;

public sealed class AirportCodeDto
{
    public AirportCodeDto(string en)
    {
        En = en;
    }

    [JsonPropertyName("en")]
    public string En { get; }
    
    [JsonPropertyName("ru")]
    public string? Ru { get; init; }
}