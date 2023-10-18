using Newtonsoft.Json;

namespace ObjectsLoader.JsonModels;

public class OsmJsonRoot
{
    [JsonProperty("elements")] public List<OsmJsonElement> Elements { get; init; } = null!;
}