using System.Text.Json.Serialization;

namespace ExternalTranslator.JsonModels;

public class MyMemoryJson
{
    [JsonPropertyName("responseData")] public MyMemoryResponseJson Response { get; init; } = null!;
}