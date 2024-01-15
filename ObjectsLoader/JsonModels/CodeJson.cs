using System.Text.Json.Serialization;

namespace ObjectsLoader.JsonModels;

public class CodeJson
{
    [JsonPropertyName("En")] public required string En { get; init; }
}