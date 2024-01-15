using System.Text.Json.Serialization;

namespace Geo.Api.V1.RailStations.Models;

public sealed class RailStationCodeDto
{
    public RailStationCodeDto(int express3)
    {
        Express3 = express3;
    }

    [JsonPropertyName("express3")]
    public int Express3 { get; }
}