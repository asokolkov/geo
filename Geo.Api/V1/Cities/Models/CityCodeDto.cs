using System.Text.Json.Serialization;

namespace Geo.Api.V1.Cities.Models;

public sealed class CityCodeDto
{
    public CityCodeDto(string en)
    {
        En = en;
    }

    [JsonPropertyName("en")]
    public string En { get; }
}