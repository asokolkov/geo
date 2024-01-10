using System.Text.Json.Serialization;

namespace Geo.Api.V1.Airports.Models;

public sealed record GetAirportDto
{
    [JsonPropertyName("id")]
    public int? Id { get; init; }
    
    [JsonPropertyName("code")]
    public string? Code { get; init; }
}