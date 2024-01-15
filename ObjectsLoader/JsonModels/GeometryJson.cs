using System.Text.Json.Serialization;

namespace ObjectsLoader.JsonModels;

public sealed class GeometryJson
{
    [JsonPropertyName("lat")] public double Latitude { get; init; }
    [JsonPropertyName("lon")] public double Longitude { get; init; }
}