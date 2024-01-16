using System.Text.Json.Serialization;
using ObjectsLoader.Extensions;

namespace ObjectsLoader.JsonModels;

public class OsmRelationElementJson
{
    [JsonPropertyName("type")] public required string Type { get; init; }
    [JsonConverter(typeof(IntToStringConverter))] [JsonPropertyName("id")] public required string Id { get; init; }
}