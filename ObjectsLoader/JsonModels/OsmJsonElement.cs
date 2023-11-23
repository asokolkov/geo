using System.Text.Json.Serialization;

namespace ObjectsLoader.JsonModels;

internal sealed class OsmJsonElement
{
    [JsonPropertyName("id")] public int OsmId { get; init; }
    [JsonPropertyName("lat")] public double Latitude { get; init; }
    [JsonPropertyName("lon")] public double Longitude { get; init; }
    [JsonPropertyName("tags")] public Dictionary<string, string> Tags { get; init; } = null!;
}