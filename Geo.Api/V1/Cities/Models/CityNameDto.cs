using System.Text.Json.Serialization;

namespace Geo.Api.V1.Cities.Models;

public sealed class CityNameDto
{
    public CityNameDto(string ru)
    {
        Ru = ru;
    }

    [JsonPropertyName("ru")]
    public string Ru { get; }
    
    [JsonPropertyName("en")]
    public string? En { get; init; }
}