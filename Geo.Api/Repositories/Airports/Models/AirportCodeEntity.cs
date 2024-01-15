using System.Text.Json.Serialization;

namespace Geo.Api.Repositories.Airports.Models;

public sealed class AirportCodeEntity
{
    public AirportCodeEntity(string en)
    {
        En = en;
    }

    [JsonPropertyName("en")]
    public string En { get; }
    
    [JsonPropertyName("ru")]
    public string? Ru { get; init; }
}