using Newtonsoft.Json;

namespace ObjectsLoader.JsonModels;

public class OsmJsonElement
{
    [JsonProperty("id")] public string OsmId { get; init; } = null!;
    [JsonProperty("lat")] public double Latitude { get; init; }
    [JsonProperty("lon")] public double Longitude { get; init; }
    [JsonProperty("tags")] public Dictionary<string, string> Tags { get; init; } = null!;
}