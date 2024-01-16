using System.Text.Json.Serialization;

namespace ObjectsLoader.JsonModels;

public class OsmRelationRootJson
{
    [JsonPropertyName("elements")] public required List<OsmRelationElementJson> Elements { get; init; }
}