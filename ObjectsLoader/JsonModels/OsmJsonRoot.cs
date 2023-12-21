using System.Text.Json.Serialization;

namespace ObjectsLoader.JsonModels;

internal sealed class OsmJsonRoot
{
    [JsonPropertyName("elements")] public required List<OsmJsonElement> Elements { get; init; }
}