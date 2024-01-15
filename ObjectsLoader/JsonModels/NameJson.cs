using System.Text.Json.Serialization;

namespace ObjectsLoader.JsonModels;

public class NameJson
{
    [JsonPropertyName("ru")] public required string Ru { get; init; }
    [JsonPropertyName("en")] public string? En { get; init; }
}