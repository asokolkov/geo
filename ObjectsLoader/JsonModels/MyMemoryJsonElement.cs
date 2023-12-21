using System.Text.Json.Serialization;

namespace ObjectsLoader.JsonModels;

internal sealed class MyMemoryJsonElement
{
    [JsonPropertyName("responseData")] public required Dictionary<string, string> ResponseData { get; init; }
}