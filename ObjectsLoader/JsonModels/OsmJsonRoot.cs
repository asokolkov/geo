using System.Text.Json.Serialization;

namespace ObjectsLoader.JsonModels;

internal sealed class OsmJsonRoot
{
    [JsonPropertyName("elements")] public List<OsmJsonElement> Elements { get; init; } = null!;
}