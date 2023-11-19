using System.Text.Json.Serialization;

namespace ObjectsLoader.JsonModels;

internal sealed class MyMemoryJsonElement
{
    [JsonPropertyName("responseData")] public Dictionary<string, string> ResponseData { get; init; } = null!;
}