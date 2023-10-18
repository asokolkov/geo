using Newtonsoft.Json;

namespace ObjectsLoader.JsonModels;

public class NominatimJsonElement
{
    [JsonProperty("osm_id")] public string OsmId { get; init; } = null!;
    [JsonProperty("address")] public Dictionary<string, string> Address { get; init; } = null!;
}