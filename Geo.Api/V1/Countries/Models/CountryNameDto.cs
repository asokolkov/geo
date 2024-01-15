using System.Text.Json.Serialization;

namespace Geo.Api.V1.Countries.Models;

public sealed class CountryNameDto
{
    public CountryNameDto(string ru)
    {
        Ru = ru;
    }

    [JsonPropertyName("ru")]
    public string Ru { get; }
    
    [JsonPropertyName("en")]
    public string? En { get; init; }
}