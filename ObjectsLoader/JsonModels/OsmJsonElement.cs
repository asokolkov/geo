using System.Text.Json.Serialization;

namespace ObjectsLoader.JsonModels;

public class OsmJsonElement
{
    [JsonPropertyName("id")] public string OsmId { get; init; } = null!;
    [JsonPropertyName("lat")] public double Latitude { get; init; }
    [JsonPropertyName("lon")] public double Longitude { get; init; }
    [JsonPropertyName("tags")] public Dictionary<string, string> Tags { get; init; } = null!;
}