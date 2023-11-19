using System.Text.Json.Serialization;

namespace ObjectsLoader.JsonModels;

public class OsmJsonRoot
{
    [JsonPropertyName("elements")] public List<OsmJsonElement> Elements { get; init; } = null!;
}